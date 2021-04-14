using AutoMapper;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.DataModels;
using DataAccessLibrary.DataModels.EditModels;
using DataAccessLibrary.Enums;
using DataAccessLibrary.UserModels;
using Ganss.XSS;
using KellermanSoftware.CompareNetObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLie.Helper.Algorithms;
using ThinBlueLie.Helper.Extensions;
using ThinBlueLie.Models;
using static DataAccessLibrary.Enums.EditEnums;
using static ThinBlueLie.Helper.Algorithms.WebsiteProfiling.WebsiteProfile;
using static ThinBlueLie.Searches.SearchClasses;

namespace ThinBlueLie.Pages
{
    public partial class Edit
    {
        internal uint TimelineinfoId;
        [Inject] UserManager<ApplicationUser> UserManager { get; set; }
        [Inject] IMapper Mapper { get; set; }
        [Inject] NavigationManager NavManager { get; set; }
        [Inject] IDataAccess Data { get; set; }
        [Inject] IJSRuntime JS { get; set; }

        SubmitModel oldInfo = new();
        public bool? EventExists = null;
        public bool EventPendingEdit = false;
        string id;
        [CascadingParameter]
        private Task<AuthenticationState> AuthState { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var uri = new Uri(NavManager.Uri);
            id = QueryHelpers.ParseQuery(uri.Query).TryGetValue("id", out var type) ? type.First() : "";
            if (!string.IsNullOrWhiteSpace(id))
            {
                TimelineinfoId = Convert.ToUInt32(id);
                userState = await AuthState;
                User = await UserManager.GetUserAsync(userState.User);
                model = await FetchDataAsync();
                if (model.Officers != null)
                {
                    for (int i = 0; i < model.Officers.Count; i++)
                    {
                        SimilarOfficers.Add(new List<SimilarPersonGeneral> { });
                    };
                    for (int i = 0; i < model.Subjects.Count; i++)
                    {
                        SimilarSubjects.Add(new List<SimilarPersonGeneral> { });
                    };

                    DateValue = Convert.ToDateTime(model.Timelineinfos.Date);
                    this.StateHasChanged();
                }
            }
        }
        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                await JS.InvokeVoidAsync("MakeMobileFriendly");
            }
            else
            {
                await JS.InvokeVoidAsync("feather.replace");
            }
        }

        internal async Task<SubmitModel> FetchDataAsync()
        {
            string checkPending = "SELECT e.Confirmed FROM edithistory e where e.IdTimelineinfo = @id Order by e.Timestamp desc Limit 1;";
            int Confirmed = await Data.LoadDataSingle<int, dynamic>(checkPending, new { id = TimelineinfoId });
            if (Confirmed == 0)
            {
                EventPendingEdit = true;
                return new SubmitModel { Timelineinfos = new ViewTimelineinfo { } };
            }
            var query = "SELECT * From timelineinfo t where t.IdTimelineinfo = @id;";
            Timelineinfo timelineinfo = await Data.LoadDataSingle<Timelineinfo, dynamic>(query, new { id = TimelineinfoId });
            //TODO change to multipleQueryAsync
            if (timelineinfo != null)
            {
                EventExists = true;
                //if post is not both locked and user missing perm to edit locked
                if (!(timelineinfo.Locked == 1 && (User.RepAuthorizer(ReputationEnum.Privileges.EditLocked) == false)))
                {
                    var mediaQuery = "SELECT *, (true) as Processed " +
                                            "From media m where m.IdTimelineinfo = @id Order By m.Rank;";
                    string misconductQuery = "SELECT * from misconducts WHERE IdTimelineinfo = @id";
                    var officerQuery = "SELECT distinct o.* FROM misconducts m " +
                                            "Join officers o on m.IdOfficer = o.IdOfficer " +
                                            "WHERE m.IdTimelineinfo = @id;";
                    var subjectQuery = "SELECT distinct o.* FROM misconducts m " +
                                            "Join subjects o on m.IdSubject = o.IdSubject " +
                                            "WHERE m.IdTimelineinfo = @id;";

                    //get media, officers, and subjects using timelineinfo id
                    List<ViewMedia> Media = await Data.LoadData<ViewMedia, dynamic>(mediaQuery, new { id = TimelineinfoId });
                    List<Misconducts> misconducts = await Data.LoadData<Misconducts, dynamic>(misconductQuery, new { id = TimelineinfoId });
                    List<Officer> officers = await Data.LoadData<Officer, dynamic>(officerQuery, new { id = TimelineinfoId });
                    List<Subject> subjects = await Data.LoadData<Subject, dynamic>(subjectQuery, new { id = TimelineinfoId });

                    Media = await ViewMedia.GetDataMany(Media);

                    var Info = Mapper.Map<Timelineinfo, ViewTimelineinfo>(timelineinfo);
                    var Misconducts = Mapper.Map<List<Misconducts>, List<ViewMisconduct>>(misconducts);
                    var Officers = Mapper.Map<List<Officer>, List<ViewOfficer>>(officers);
                    var Subjects = Mapper.Map<List<Subject>, List<ViewSubject>>(subjects);

                    officers.SetAgeFromDOB(timelineinfo.Date);
                    subjects.SetAgeFromDOB(timelineinfo.Date);
                    for (int i = 0; i < Officers.Count; i++)
                    {
                        Officers[i].Rank = i;
                    }
                    for (int i = 0; i < Subjects.Count; i++)
                    {
                        Subjects[i].Rank = i;
                    }


                    oldInfo = new SubmitModel
                    {
                        Timelineinfos = Info,
                        Misconducts = Misconducts,
                        Medias = Media,
                        Officers = Officers,
                        Subjects = Subjects
                    };
                    SubmitModel model = new();
                    Mapper.Map(oldInfo, model);
                    EventExists = true;
                    return model;
                }
            }
            EventExists = false;
            return new SubmitModel { Timelineinfos = new ViewTimelineinfo { Locked = timelineinfo.Locked } };
        }


        public EditHistory editHistory;
        async Task CreateEmptyEditHistory()
        {
            if (editHistory.ContainsChange() && (CreatedNewEditHistory == false))
            {
                string createNewEditHistory = @"INSERT INTO edithistory (`Confirmed`, `SubmittedBy`, `IdTimelineinfo`) 
                                                        VALUES ('2', @userId, @IdTimelineinfo);
                                            SELECT LAST_INSERT_ID();";
                EditHistoryId = await Data.LoadDataSingle<int, dynamic>(createNewEditHistory, new { userId, IdTimelineinfo = TimelineinfoId });
                CreatedNewEditHistory = true;
            }
        }
        bool CreatedNewEditHistory;

        public bool SavingData = false;

        internal AuthenticationState userState;
        int userId;
        int EditHistoryId;
        public ApplicationUser User;
        internal async void HandleValidSubmitAsync()
        {
            CreatedNewEditHistory = false;
            SavingData = true;
            StateHasChanged();
            userId = Convert.ToInt32(UserManager.GetUserId(userState.User));

            editHistory = new EditHistory();
            PairVersions Pair = new();
            CompareLogic compareLogic = new();


            if (!compareLogic.Compare(model.Subjects, oldInfo.Subjects).AreEqual)
            {
                for (int i = 0; i < model.Subjects.Count; i++)
                {
                    var subject = model.Subjects[i];
                    if (subject.IdSubject == 0)
                    {
                        subject.DOB = subject.Age?.AgeToDOB(model.Timelineinfos.Date);
                        var subjectSql = @"SET @v1 = (SELECT COALESCE(Max(e.IdSubject +1),1) FROM edits_subject e);                                               
                                                INSERT INTO edits_subject 
                                                  (`IdEditHistory`, `IdSubject`, `Name`, `Race`, `Sex`, `DOB`, `Action`)
                                                   VALUES (@IdEditHistory, @v1, @Name, @Race, @Sex, @DOB, '0');
                                                SELECT @v1;";
                        //Add to subjects table and return id
                        dynamic editSubject = subject;
                        editSubject.IdEditHistory = EditHistoryId;
                        var subjectId = await Data.SaveDataAndReturn<dynamic>(subjectSql, editSubject);
                        model.Misconducts.Where(m => m.Subject == subject).FirstOrDefault().Subject.IdSubject = subjectId;
                        model.Subjects[i].IdSubject = subjectId;
                    }
                }
            }
            if (!compareLogic.Compare(model.Officers, oldInfo.Officers).AreEqual)
            {
                for (int i = 0; i < model.Officers.Count; i++)
                {
                    var officer = model.Officers[i];
                    if (officer.IdOfficer == 0)
                    {
                        officer.DOB = officer.Age?.AgeToDOB(model.Timelineinfos.Date);
                        var officerSql = @"SET @v1 = (SELECT COALESCE(Max(e.IdOfficer +1),1) FROM edits_officer e);                                               
                                                INSERT INTO edits_officer 
                                                  (`IdEditHistory`, `IdOfficer`, `Name`, `Race`, `Sex`, `DOB`, `Action`)
                                                   VALUES (@IdEditHistory, @v1, @Name, @Race, @Sex, @DOB, '0');
                                                SELECT @v1;";
                        //Add to officers table and return id
                        dynamic editOfficer = officer;
                        editOfficer.IdEditHistory = EditHistoryId;
                        var officerId = await Data.SaveDataAndReturn<dynamic>(officerSql, editOfficer);
                        model.Misconducts.Where(m => m.Officer == officer).FirstOrDefault().Officer.IdOfficer = officerId;
                        model.Officers[i].IdOfficer = officerId;
                    }
                }
            }
            if (!compareLogic.Compare(model.Misconducts, oldInfo.Misconducts).AreEqual)
            {
                editHistory.Misconducts = 1; //triggers editHistory's set                
                await CreateEmptyEditHistory();
                var editMisconducts = Mapper.Map<List<ViewMisconduct>, List<EditMisconducts>>(model.Misconducts);
                editMisconducts.ForEach(m => { m.IdTimelineinfo = (int)TimelineinfoId; m.IdEditHistory = EditHistoryId; });
                string newTimelineinfoSubject = 
                    "INSERT INTO edit_misconducts (`IdEditHistory`, `IdTimelineinfo`, `IdOfficer`, `IdSubject`, `Misconduct`, `Weapon`, `Armed`, `SWAT`) " +
                             "VALUES (@IdEditHistory, @IdTimelineinfo, @IdOfficer, @IdSubject, @Misconduct, @Weapon, @Armed, @SWAT);";
                await Data.SaveData(newTimelineinfoSubject, editMisconducts);
            }

            if (!compareLogic.Compare(model.Medias, oldInfo.Medias).AreEqual)
            {
                editHistory.EditMedia = 1;
                await CreateEmptyEditHistory();
                //giving it a random id that will be used for pairing, but never inserted into database
                foreach (var media in model.Medias.Where(subject => subject.IdMedia == 0))
                    media.IdMedia = (int)(int.MaxValue - new Random().Next(10000000));
                var mediaPairs = Pair.PairMedia(oldInfo.Medias, model.Medias);
                foreach (var pair in mediaPairs)
                {
                    //If there is a change
                    if (!compareLogic.Compare(pair.Item1, pair.Item2).AreEqual)
                    {
                        EditActions Action;
                        //if new media
                        if (pair.Item2 != null && pair.Item1 == null)
                        {
                            Action = EditActions.Addition;
                            //TODO add support for editted uploaded images
                            var result = await PrepareStoreData(pair.Item2);
                            pair.Item2.SourcePath = result.SourcePath;
                            pair.Item2.SourceFrom = result.SourceFrom;
                            pair.Item2.Blurb = pair.Item2.Blurb.Trim();
                            string saveNewMedia = @$"INSERT INTO editmedia 
                                                      (`IdEditHistory`, `IdTimelineinfo`, `MediaType`, `SourcePath`, `Thumbnail`, `OriginalUrl`,
                                                          `Gore`, `SourceFrom`, `Blurb`, `Credit`, `SubmittedBy`, `Rank`, `Action`)
                                                       VALUES ('{EditHistoryId}', '{TimelineinfoId}', @MediaType, @SourcePath, @Thumbnail, @OriginalUrl,
                                                          @Gore, @SourceFrom, @Blurb, @Credit, '{userId}', @Rank, '{(int)Action}');";
                            await Data.SaveData(saveNewMedia, pair.Item2);
                        }
                        //if deleted media
                        else if (pair.Item2 == null && pair.Item1 != null)
                        {
                            Action = EditActions.Deletion;
                            string deleteMedia = $@"INSERT INTO editmedia (`IdEditHistory`, `IdTimelineinfo`, `IdMedia`, `SubmittedBy`, `Action`) 
                                                    VALUES ('{EditHistoryId}', '{TimelineinfoId}', '{pair.Item1.IdMedia}', '{userId}', '{(int)Action}');";
                            await Data.SaveData(deleteMedia, new { });
                        }
                        //if updated
                        else
                        {
                            Action = EditActions.Update;
                            pair.Item2.Blurb = pair.Item2.Blurb.Trim();
                            string updateMedia = $@"INSERT INTO editmedia 
                                                      (`IdEditHistory`, `IdTimelineinfo`, `MediaType`, `SourcePath`, `Thumbnail`, `OriginalUrl`,
                                                          `Gore`, `SourceFrom`, `Blurb`, `Credit`, `SubmittedBy`, `Rank`, `Action`)
                                                       VALUES ('{EditHistoryId}', '{TimelineinfoId}', @MediaType, @SourcePath, @Thumbnail, @OriginalUrl,
                                                          @Gore, @SourceFrom, @Blurb, @Credit, '{userId}', @Rank, '{(int)Action}');";
                            await Data.SaveData(updateMedia, pair.Item2);
                        }
                    }
                }
            }
            if (!compareLogic.Compare(model.Timelineinfos, oldInfo.Timelineinfos).AreEqual)
            {
                editHistory.Edits = 1;
                await CreateEmptyEditHistory();
                var sanitizer = new HtmlSanitizer();
                //TODO this currently doesnt do anything, have to initialize the sanitizer object with this info
                sanitizer.AllowedCssProperties.Remove("color");
                sanitizer.AllowedCssProperties.Remove("display");
                sanitizer.AllowedCssProperties.Remove("font-style");
                sanitizer.AllowedCssProperties.Remove("font-family");
                sanitizer.AllowedCssProperties.Remove("background-color");
                sanitizer.AllowedCssProperties.Remove("whitespace");
                model.Timelineinfos.Title = model.Timelineinfos.Title.Trim();
                model.Timelineinfos.City = model.Timelineinfos.City.Trim();
                model.Timelineinfos.Context = sanitizer.Sanitize(model.Timelineinfos.Context);
                string InsertEdits = $@"INSERT INTO edits (`IdEditHistory`, `IdTimelineinfo`, `Title`, `Date`, `State`, 
                                           `City`, `Context`, `Locked`) 
                                        VALUES ('{EditHistoryId}', '{TimelineinfoId}', @Title, @Date, @State,
                                            @City, @Context, @Locked);";
                await Data.SaveData(InsertEdits, model.Timelineinfos);
            }

            string updateEditHistory = @$"UPDATE edithistory SET 
                                               `Confirmed` = '0',`Edits` = @Edits, `EditMedia` = @EditMedia,
                                                `Officers` = @Officers, `Subjects` = @Subjects, `Misconducts` = @Misconducts
                                         WHERE (`IdEditHistory` = '{EditHistoryId}');";
            await Data.SaveData(updateEditHistory, editHistory);
            Serilog.Log.Information("Created new Edit {EditChanges} for Event {id}", compareLogic.Compare(model, oldInfo).Differences, TimelineinfoId);
            NavManager.NavigateTo("/Account/Profile");
            SavingData = false;
        }
    }
}
