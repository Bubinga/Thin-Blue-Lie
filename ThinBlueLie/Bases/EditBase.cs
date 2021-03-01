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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLie.Helper.Algorithms;
using ThinBlueLie.Helper.Extensions;
using ThinBlueLie.Models;
using static DataAccessLibrary.Enums.EditEnums;
using static ThinBlueLie.Helper.Algorithms.WebsiteProfiling.WebsiteProfile;
using static ThinBlueLie.Helper.ConfigHelper;
using static ThinBlueLie.Models.SubmitBase;

namespace ThinBlueLie.Bases
{
    public class EditBase : InformationBase
    {
        //public new SubmitModel model = new SubmitModel
        //{
        //    Timelineinfos = new ViewTimelineinfo(),
        //    Medias = new List<ViewMedia> { new ViewMedia { } },
        //    Officers = new List<ViewOfficer> { new ViewOfficer { } },
        //    Subjects = new List<ViewSubject> { new ViewSubject { } }
        //};
        internal uint Id;
        [Inject]
        SignInManager<ApplicationUser> SignInManager { get; set; }
        [Inject]
        UserManager<ApplicationUser> UserManager { get; set; }
        [Inject]
        public IMapper Mapper { get; set; }
        [Inject]
        NavigationManager navManager { get; set; }
        [Inject]
        IDataAccess Data { get; set; }


        SubmitModel oldInfo = new SubmitModel();

        public bool? EventExists = null;

        public bool EventPendingEdit = false;
        internal async Task<SubmitModel> FetchDataAsync()
        {
            string checkPending = "SELECT e.Confirmed FROM edithistory e where e.IdTimelineinfo = @id Order by e.Timestamp desc Limit 1;";
            int Confirmed = await Data.LoadDataSingle<int, dynamic>(checkPending, new { id = Id });
            if (Confirmed == 0) { 
                EventPendingEdit = true;
                return new SubmitModel { Timelineinfos = new ViewTimelineinfo { } }; 
            }
            var query = "SELECT * From timelineinfo t where t.IdTimelineinfo = @id;";
            Timelineinfo timelineinfo = await Data.LoadDataSingle<Timelineinfo, dynamic>(query, new { id = Id });
            //TODO change to multipleQueryAsync
            if (timelineinfo != null)
            {
                EventExists = true;
                //if post is not both locked and user missing perm to edit locked
                if (!(timelineinfo.Locked == 1 && (User.RepAuthorizer(PrivilegeEnum.Privileges.EditLocked) == false)))
                {
                    var mediaQuery = "SELECT *, (true) as Processed " +
                                            "From media m where m.IdTimelineinfo = @id Order By m.Rank;";
                    var officerQuery = "SELECT o.*, t_o.Age, t_o.Misconduct, t_o.Weapon " +
                            "FROM timelineinfo t " +
                            "JOIN timelineinfo_officer t_o ON t.IdTimelineinfo = t_o.IdTimelineinfo " +
                            "JOIN officers o ON t_o.IdOfficer = o.IdOfficer " +
                            "WHERE t.IdTimelineinfo = @id ;";
                    var subjectQuery = "SELECT s.*, t_s.Age, t_s.Armed " +
                            "FROM timelineinfo t " +
                            "JOIN timelineinfo_subject t_s ON t.IdTimelineinfo = t_s.IdTimelineinfo " +
                            "JOIN subjects s ON t_s.IdSubject = s.IdSubject " +
                            "WHERE t.IdTimelineinfo = @id;";

                    //get media, officers, and subjects using timelineinfo id
                    List<ViewMedia> Media = await Data.LoadData<ViewMedia, dynamic>(mediaQuery, new { id = Id });
                    List<DBOfficer> officers = await Data.LoadData<DBOfficer, dynamic>(officerQuery, new { id = Id });
                    List<DBSubject> subjects = await Data.LoadData<DBSubject, dynamic>(subjectQuery, new { id = Id });

                    Media = await ViewMedia.GetDataMany(Media);

                    var Info = Mapper.Map<Timelineinfo, ViewTimelineinfo>(timelineinfo);
                    var Officers = Mapper.Map<List<DBOfficer>, List<ViewOfficer>>(officers);
                    var Subjects = Mapper.Map<List<DBSubject>, List<ViewSubject>>(subjects);
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
                        Medias = Media,
                        Officers = Officers,
                        Subjects = Subjects
                    };
                    SubmitModel model = new SubmitModel();
                    Mapper.Map(oldInfo, model);
                    EventExists = true;
                    return model;
                }
            }
            EventExists = false;
            return new SubmitModel { Timelineinfos = new ViewTimelineinfo {Locked = timelineinfo.Locked } };
        }



        public EditHistory editHistory;
        async Task CreateEmptyEditHistory()
        {
            if (editHistory.ContainsChange() && (CreatedNewEditHistory == false))
            {
                string createNewEditHistory = @"INSERT INTO edithistory (`Confirmed`, `SubmittedBy`, `IdTimelineinfo`) 
                                                        VALUES ('2', @userId, @IdTimelineinfo);
                                            SELECT LAST_INSERT_ID();";
                EditHistoryId = await Data.LoadDataSingle<int, dynamic>(createNewEditHistory, new { userId, IdTimelineinfo = Id });
                CreatedNewEditHistory = true;
            }
        }
        bool CreatedNewEditHistory;

        public bool SavingData = false;

        internal AuthenticationState userState;
        int userId;
        int EditHistoryId;
        public ApplicationUser User;
        //TODO only add to junction tables is something changes. 
        internal async void HandleValidSubmitAsync()
        {
            CreatedNewEditHistory = false;
            SavingData = true;
            StateHasChanged();
            userId = Convert.ToInt32(UserManager.GetUserId(userState.User));

            editHistory = new EditHistory();
            PairVersions Pair = new PairVersions();
            CompareLogic compareLogic = new CompareLogic();

            if (!compareLogic.Compare(model.Officers, oldInfo.Officers).AreEqual)
            {
                foreach (var officer in model.Officers.Where(subject => subject.IdOfficer == 0))
                    officer.IdOfficer = int.MaxValue - new Random().Next(10000000);

                var officerPairs = Pair.PairOfficers(Mapper.Map<List<ViewOfficer>, List<DBOfficer>>(oldInfo.Officers),
                                                           Mapper.Map<List<ViewOfficer>, List<DBOfficer>>(model.Officers));
                bool ChangedJunction = false;
                foreach (var pair in officerPairs)
                {
                    //if officer changed and the change was not a deletion
                    if (pair.Item2 != null &&
                        Mapper.Map<DBOfficer, CommonPerson>(pair.Item2).PersonChange(Mapper.Map<DBOfficer, CommonPerson>(pair.Item1)))
                    {
                        //create new edithistory 
                        string sql = @"INSERT INTO edithistory (`SubmittedBy`, `Officers`)
                                                  VALUES (@userId, '1');
                                                  Set @editHistoryId = (Select LAST_INSERT_ID());
                                       INSERT INTO edits_officer
                                                 (`IdEditHistory`, `IdOfficer`, `Name`, `Race`, `Sex`, `Image`, `Local`, `Action`) 
                                                 VALUES (@editHistoryId, @IdOfficer, @Name, @Race, @Sex, @Image, @Local, @Action);";
                        EditActions Action;
                        if (pair.Item1 == null)
                            Action = EditActions.Addition;
                        //if (pair.Item2 == null)  Not the place to do deletion of person
                        //    Action = EditActions.Deletion;
                        else
                            Action = EditActions.Update;
                        await Data.SaveData(sql, new
                        {
                            userId = userId,
                            IdOfficer = pair.Item2?.IdOfficer,
                            Name = pair.Item2?.Name.Trim(),
                            Race = (int?)(pair.Item2?.Race ?? 0),
                            Sex = (int?)(pair.Item2?.Sex ?? 0),
                            Image = pair.Item1?.Image ?? pair.Item2?.Image,
                            Local = pair.Item1?.Local ?? pair.Item2?.Local,
                            Action = (int)Action
                        });
                    }
                    if ((pair.Item1?.Age != pair.Item2?.Age) || (pair.Item1?.Misconduct != pair.Item2?.Misconduct)
                        || (pair.Item1?.Weapon != pair.Item2?.Weapon))
                        ChangedJunction = true;
                }
                if (ChangedJunction)
                {
                    editHistory.Timelineinfo_Officer = 1; 
                    await CreateEmptyEditHistory();               
                    
                    foreach (var officer in model.Officers)
                    {
                        var weapon = officer.Weapon?.Sum() == 0 ? null : officer.Weapon?.Sum();
                        string newTimelineinfoOfficer = $@"INSERT INTO edits_timelineinfo_officer
                                                        (`IdEditHistory`, `IdTimelineinfo`, `IdOfficer`, `Misconduct`, `Weapon`, `Age`) 
                                                        VALUES ('{EditHistoryId}', '{Id}', @IdOfficer, '{officer.Misconduct.Sum()}', '{weapon}', @Age);";
                        await Data.SaveData(newTimelineinfoOfficer, officer);
                    }                    
                }
            }
            if (!compareLogic.Compare(model.Subjects, oldInfo.Subjects).AreEqual)
            {
                foreach (var subject in model.Subjects.Where(subject => subject.IdSubject == 0))
                    subject.IdSubject = int.MaxValue - new Random().Next(10000000);

                var subjectPairs = Pair.PairSubjects(Mapper.Map<List<ViewSubject>, List<DBSubject>>(oldInfo.Subjects),
                                                           Mapper.Map<List<ViewSubject>, List<DBSubject>>(model.Subjects));
                bool ChangedJunction = false;
                foreach (var pair in subjectPairs)
                {

                    //if subject changed and the change was not a deletion
                    if (pair.Item2 != null &&
                        Mapper.Map<DBSubject, CommonPerson>(pair.Item2).PersonChange(Mapper.Map<DBSubject, CommonPerson>(pair.Item1)))
                    {
                        string sql = @"INSERT INTO edithistory (`SubmittedBy`, `Subjects`)
                                                  VALUES (@userId, '1');
                                                  Set @editHistoryId = (Select LAST_INSERT_ID());
                                       INSERT INTO edits_subject
                                                 (`IdEditHistory`, `IdSubject`, `Name`, `Race`, `Sex`, `Image`, `Local`, `Action`) 
                                                 VALUES (@editHistoryId, @IdSubject, @Name, @Race, @Sex, @Image, @Local, @Action);";
                        EditActions Action;
                        if (pair.Item1 == null)
                            Action = EditActions.Addition;
                        //if (pair.Item2 == null)
                        //    Action = EditActions.Deletion; not where person deletion should happen
                        else
                            Action = EditActions.Update;
                        await Data.SaveData(sql, new
                        {
                            userId = userId,
                            IdSubject = pair.Item2?.IdSubject,
                            Name = pair.Item2?.Name.Trim(),
                            Race = (int?)(pair.Item2?.Race ?? 0), // should never be null anyways
                            Sex = (int?)(pair.Item2?.Sex ?? 0),
                            Image = pair.Item1?.Image ?? pair.Item2?.Image,
                            Local = pair.Item1?.Local ?? pair.Item2?.Local,
                            Action = (int)Action
                        });
                    }
                    //junction table has to change
                    if ((pair.Item1?.Age != pair.Item2?.Age) || (pair.Item1?.Armed != pair.Item2?.Armed))
                        ChangedJunction = true;
                }
                if (ChangedJunction)
                {
                    editHistory.Timelineinfo_Subject = 1; //triggers editHistory's set
                    await CreateEmptyEditHistory();
                    string newTimelineinfoSubject = $@"INSERT INTO edits_timelineinfo_subject
                                                        (`IdEditHistory`, `IdTimelineinfo`, `IdSubject`, `Armed`, `Age`) 
                                                        VALUES ('{EditHistoryId}', '{Id}', @IdSubject, @Armed, @Age);";
                    await Data.SaveData(newTimelineinfoSubject, model.Subjects);
                }
            }
            if (!compareLogic.Compare(model.Medias, oldInfo.Medias).AreEqual)
            {
                editHistory.EditMedia = 1;
                await CreateEmptyEditHistory();
                //giving it a random id that will be used for pairing, but never inserted into database
                foreach (var media in model.Medias.Where(subject => subject.IdMedia == 0))
                    media.IdMedia = int.MaxValue - new Random().Next(10000000);
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
                                                      (`IdEditHistory`, `IdTimelineinfo`, `Rank`, `MediaType`, `SourcePath`, `OriginalUrl`,
                                                        `Gore`, `SourceFrom`, `Blurb`, `Credit`, `SubmittedBy`, `Action`)
                                                       VALUES ('{EditHistoryId}', '{Id}', @Rank, @MediaType, @SourcePath, @OriginalUrl,
                                                          @Gore, @SourceFrom, @Blurb, @Credit, '{userId}', '{(int)Action}');";
                            await Data.SaveData(saveNewMedia, pair.Item2);
                        }
                        //if deleted media
                        else if (pair.Item2 == null && pair.Item1 != null)
                        {
                            Action = EditActions.Deletion;
                            string deleteMedia = $@"INSERT INTO editmedia (`IdEditHistory`, `IdTimelineinfo`, `IdMedia`, `SubmittedBy`, `Action`) 
                                                    VALUES ('{EditHistoryId}', '{Id}', '{pair.Item1.IdMedia}', '{userId}', '{(int)Action}');";
                            await Data.SaveData(deleteMedia, new { });
                        }
                        //if updated
                        else
                        {
                            Action = EditActions.Update;
                            pair.Item2.Blurb = pair.Item2.Blurb.Trim();
                            string updateMedia = $@"INSERT INTO editmedia 
                                                      (`IdEditHistory`, `IdTimelineinfo`, `IdMedia`, `Rank`, `MediaType`, `SourcePath`, `OriginalUrl`,
                                                        `Gore`, `SourceFrom`, `Blurb`, `Credit`, `SubmittedBy`, `Action`)
                                                       VALUES ('{EditHistoryId}', '{Id}', @IdMedia, @Rank, @MediaType, @SourcePath, @OriginalUrl,
                                                          @Gore, @SourceFrom, @Blurb, @Credit, '{userId}', '{(int)Action}');";
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
                                        VALUES ('{EditHistoryId}', '{Id}', @Title, @Date, @State,
                                            @City, @Context, @Locked);";
                await Data.SaveData(InsertEdits, model.Timelineinfos);
            }

            string updateEditHistory = @$"UPDATE edithistory SET 
                                               `Confirmed` = '0',`Edits` = @Edits, `EditMedia` = @EditMedia,
                                                `Officers` = @Officers, `Subjects` = @Subjects, `Timelineinfo_Officer` = @Timelineinfo_Officer, 
                                                `Timelineinfo_Subject` = @Timelineinfo_Subject 
                                         WHERE (`IdEditHistory` = '{EditHistoryId}');";
            await Data.SaveData(updateEditHistory, editHistory);
            Serilog.Log.Information("Created new Edit {EditChanges} for Event {id}", compareLogic.Compare(model, oldInfo).Differences, Id);
            navManager.NavigateTo("/Account/Profile");
            SavingData = false;
        }
    }
}
