using AutoMapper;
using Dapper;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.DataModels;
using DataAccessLibrary.DataModels.EditModels;
using DataAccessLibrary.Enums;
using DataAccessLibrary.UserModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLieB.Helper;
using ThinBlueLieB.Helper.Algorithms;
using ThinBlueLieB.Helper.Extensions;
using ThinBlueLieB.Models;
using ThinBlueLieB.ViewModels;
using static DataAccessLibrary.Enums.EditEnums;
using static ThinBlueLieB.Helper.ConfigHelper;
using static ThinBlueLieB.Models.SubmitBase;

namespace ThinBlueLieB.Bases
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


        SubmitModel oldInfo = new SubmitModel();

        public bool EventExists = false;
        
        internal async Task<SubmitModel> FetchDataAsync()
        {
            DataAccess data = new DataAccess();
            var query = "SELECT * From timelineinfo t where t.IdTimelineinfo = @id;";
            Timelineinfo timelineinfo = await data.LoadDataSingle<Timelineinfo, dynamic>(query, new { id = Id }, GetConnectionString());
            if (timelineinfo != null)
            {
                var mediaQuery = "SELECT m.MediaType, m.SourcePath, m.Gore, m.SourceFrom, m.Blurb, m.Credit, m.SubmittedBy, m.Rank From media m where m.IdTimelineinfo = @id Order By m.Rank;";
                var officerQuery = "SELECT o.Name, o.Race, o.Sex, t_o.Age, t_o.Misconduct, t_o.Weapon " +
                        "FROM timelineinfo t " +
                        "JOIN timelineinfo_officer t_o ON t.IdTimelineinfo = t_o.IdTimelineinfo " +
                        "JOIN officers o ON t_o.IdOfficer = o.IdOfficer " +
                        "WHERE t.IdTimelineinfo = @id ;";
                var subjectQuery = "SELECT s.Name, s.Race, s.Sex, t_s.Age, t_s.Armed " +
                        "FROM timelineinfo t " +
                        "JOIN timelineinfo_subject t_s ON t.IdTimelineinfo = t_s.IdTimelineinfo " +
                        "JOIN subjects s ON t_s.IdSubject = s.IdSubject " +
                        "WHERE t.IdTimelineinfo = @id;";

                //get media, officers, and subjects using timelineinfo id
                List<DisplayMedia> media = await data.LoadData<DisplayMedia, dynamic>(mediaQuery, new { id = Id }, GetConnectionString());
                List<DBOfficer> officers = await data.LoadData<DBOfficer, dynamic>(officerQuery, new { id = Id }, GetConnectionString());
                List<DBSubject> subjects = await data.LoadData<DBSubject, dynamic>(subjectQuery, new { id = Id }, GetConnectionString());
                var Info = new ViewTimelineinfo
                {
                    Date = timelineinfo.Date.ToString("yyyy-MM-dd"),
                    State = (TimelineinfoEnums.StateEnum?)timelineinfo.State,
                    City = timelineinfo.City,
                    Context = timelineinfo.Context,
                    Locked = timelineinfo.Locked,
                    SubmittedBy = timelineinfo.Owner
                };
                var Media = Mapper.Map<List<DisplayMedia>, List<ViewMedia>>(media);
                var Officers = Mapper.Map<List<DBOfficer>, List<ViewOfficer>>(officers);
                var Subjects = Mapper.Map<List<DBSubject>, List<ViewSubject>>(subjects);
                for (int i = 0; i < Media.Count; i++)
                {
                    Media[i].ListIndex = i;
                }

                oldInfo = model = new SubmitModel
                {
                    Timelineinfos = Info,
                    Medias = Media,
                    Officers = Officers,
                    Subjects = Subjects
                };
                EventExists = true;
                return model;
            }
            EventExists = false;
            return new SubmitModel { Timelineinfos = new ViewTimelineinfo() };
        }


        EditHistory editHistory
        {
            get { return editHistory; }
            set {
                editHistory = value;
                if (editHistory.ContainsChange() && (CreatedNewEditHistory == false))
                {
                    CreateEmptyEditHistory();
                    return;
                }
                return; }
        }
        async void CreateEmptyEditHistory()
        {
            string createNewEditHistory = @"INSERT INTO edithistory (`Confirmed`, `SubmittedBy`, `IdTimelineinfo`) 
                                                        VALUES ('2', @userId, @IdTimelineinfo);
                                            Select Last_Insert_Id();";
            EditHistoryId = await data.SaveDataAndReturn(createNewEditHistory, new { userId, IdTimelineinfo = model.Timelineinfos.IdTimelineinfo }, GetConnectionString());
            CreatedNewEditHistory = true;
        }
        bool CreatedNewEditHistory = false;

        public bool SavingData = false;

        internal AuthenticationState userState;
        DataAccess data = new DataAccess();
        int userId;
        int EditHistoryId;
        //TODO only add to junction tables is something changes. 
        internal async void HandleValidSubmitAsync()
        {
            SavingData = true;
            var medias = Mapper.Map<List<ViewMedia>, List<EditMedia>>(model.Medias);
            userId = Convert.ToInt32(UserManager.GetUserId(userState.User));

            editHistory = new EditHistory();
            PairVersions Pair = new PairVersions();

            if (model.Officers != oldInfo.Officers)
            {
                foreach (var officer in model.Officers.Where(subject => subject.IdOfficer == 0))
                    officer.IdOfficer = int.MaxValue - new Random().Next(1000000);

                var officerPairs = Pair.PairOfficers(Mapper.Map<List<ViewOfficer>, List<DBOfficer>>(oldInfo.Officers), 
                                                           Mapper.Map<List<ViewOfficer>, List<DBOfficer>>(model.Officers));
                bool ChangedJunction = false;
                foreach (var pair in officerPairs)
                {
                    //if officer changed
                    if (Mapper.Map<DBOfficer, CommonPerson>(pair.Item2).JunctionChange(Mapper.Map<DBOfficer, CommonPerson>(pair.Item1)))
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
                        if (pair.Item2 == null)
                            Action = EditActions.Deletion;
                        else
                            Action = EditActions.Update;
                        await data.SaveData(sql, new
                        {
                            userId = userId,
                            IdOfficer = pair.Item2.IdOfficer,
                            Name = pair.Item2.Name,
                            Race = (int)pair.Item2.Race,
                            Image = pair.Item2.Image,
                            Local = pair.Item2.Local,
                            Action = (int)Action
                        },
                                GetConnectionString());
                    }
                    if ( (pair.Item1?.Age != pair.Item2?.Age) || (pair.Item1?.Misconduct != pair.Item2?.Misconduct) 
                        || (pair.Item1?.Weapon != pair.Item2?.Weapon) )
                           ChangedJunction = true;
                }
                if (ChangedJunction)
                {
                    editHistory.Timelineinfo_Officer = 1; //triggers editHistory's set
                    string newTimelineinfoOfficer = $@"INSERT INTO edits_timelineinfo_officer
                                                        (`IdEditHistory`, `IdTimelineinfo`, `IdOfficer`, `Misconduct`, `Weapon`, `Age`) 
                                                        VALUES ('{EditHistoryId}', @IdTimelineinfo, @IdOfficer, @Misconduct, @Weapon, @Age);";
                    await data.SaveData(newTimelineinfoOfficer, model.Officers, GetConnectionString());                   
                }
            }
            if (model.Subjects != oldInfo.Subjects)
            {
                foreach (var subject in model.Subjects.Where(subject => subject.IdSubject == 0))
                    subject.IdSubject = int.MaxValue - new Random().Next(1000000);

                var subjectPairs = Pair.PairSubjects(Mapper.Map<List<ViewSubject>, List<DBSubject>>(oldInfo.Subjects),
                                                           Mapper.Map<List<ViewSubject>, List<DBSubject>>(model.Subjects));
                bool ChangedJunction = false;
                foreach (var pair in subjectPairs)
                {

                    //if subject changed
                    if (Mapper.Map<DBSubject, CommonPerson>(pair.Item2).JunctionChange(Mapper.Map<DBSubject, CommonPerson>(pair.Item1)))
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
                        if (pair.Item2 == null)
                            Action = EditActions.Deletion;
                        else
                            Action = EditActions.Update;
                        await data.SaveData(sql, new {
                                userId = userId,
                                IdSubject = pair.Item2.IdSubject,
                                Name = pair.Item2.Name,
                                Race = (int)pair.Item2.Race,
                                Image = pair.Item2.Image,
                                Local = pair.Item2.Local,
                                Action = (int)Action},
                                GetConnectionString());
                    }
                    //junction table has to change
                    if ((pair.Item1?.Age != pair.Item2?.Age) || (pair.Item1?.Armed != pair.Item2?.Armed))
                        ChangedJunction = true;
                }
                if (ChangedJunction)
                {
                    editHistory.Timelineinfo_Subject = 1; //triggers editHistory's set
                    string newTimelineinfoSubject = $@"INSERT INTO edits_timelineinfo_subject
                                                        (`IdEditHistory`, `IdTimelineinfo`, `IdSubject`, `Misconduct`, `Weapon`, `Age`) 
                                                        VALUES ('{EditHistoryId}', @IdTimelineinfo, @IdSubject, @Misconduct, @Weapon, @Age);";
                    await data.SaveData(newTimelineinfoSubject, model.Subjects, GetConnectionString());
                }
            }
            if (model.Medias != oldInfo.Medias)
            {
                editHistory.EditMedia = 1;
                foreach (var media in model.Medias.Where(subject => subject.IdMedia == 0))
                    media.IdMedia = int.MaxValue - new Random().Next(1000000);
                var mediaPairs = Pair.PairMedia(Mapper.Map<List<ViewMedia>, List<Media>>(oldInfo.Medias), 
                                                    Mapper.Map<List<ViewMedia>, List<Media>>(model.Medias));
                foreach (var pair in mediaPairs)
                {
                    //If there is a change
                    if (pair.Item2 != pair.Item1)
                    {
                        EditActions Action;
                        //if new media
                        if (pair.Item2 == null && pair.Item1 != null)
                        {
                            Action = EditActions.Addition;
                            string saveNewMedia = @$"INSERT INTO editmedia 
                                                      (`IdEditHistory`, `IdTimelineinfo`, `Rank`, `MediaType`, `SourcePath`,
                                                        `Gore`, `SourceFrom`, `Blurb`, `Credit`, `SubmittedBy`, `Action`)
                                                       VALUES ('{EditHistoryId}', '{Id}', @Rank, @MediaType, @SourcePath,
                                                          @Gore, @SourceFrom, @Blurb, @Credit, '{userId}', '{(int)Action}');";
                            await data.SaveData(saveNewMedia, pair.Item2, GetConnectionString());
                        }
                        //if deleted media
                        else if (pair.Item2 == null && pair.Item1 != null)
                        {
                            Action = EditActions.Deletion;
                            string deleteMedia = $@"INSERT INTO editmedia (`IdEditHistory`, `IdTimelineinfo`, `SubmittedBy`, `Action`) 
                                                    VALUES ('{EditHistoryId}', '{Id}', '{userId}', '{(int)Action}');";
                            await data.SaveData(deleteMedia, new { }, GetConnectionString());
                        }
                        //if updated
                        else
                        {
                            Action = EditActions.Update;
                            string updateMedia = $@"INSERT INTO editmedia 
                                                      (`IdEditHistory`, `IdTimelineinfo`, `IdMedia`, `Rank`, `MediaType`, `SourcePath`,
                                                        `Gore`, `SourceFrom`, `Blurb`, `Credit`, `SubmittedBy`, `Action`)
                                                       VALUES ('{EditHistoryId}', '{Id}', @IdMedia, @Rank, @MediaType, @SourcePath,
                                                          @Gore, @SourceFrom, @Blurb, @Credit, '{userId}', '{(int)Action}";
                            await data.SaveData(updateMedia, pair.Item2, GetConnectionString());
                        }
                    }
                }               
            }
            if (model.Timelineinfos != oldInfo.Timelineinfos)
            {
                editHistory.Edits = 1;
                string InsertEdits = $@"INSERT INTO edits (`IdEditHistory`, `IdTimelineinfo`, `Title`, `Date`, `State`, 
                                           `City`, `Context`, `Locked`, `Timestamp`) 
                                        VALUES ('{EditHistoryId}', '{Id}', @Title, @Date, @State,
                                            @City, @Context, @Locked, @Timestamp);";
                await data.SaveData(InsertEdits, model.Timelineinfos, GetConnectionString());                
            }

            string updateEditHistory = @$"UPDATE edithistory SET 
                                            `Edits` = @Edits, `EditMedia` = @EditMedia, `Officers` = @Officers, `Subjects` = @Subjects, 
                                            `Timelineinfo_Officer` = @Timelineinfo_Officer, `Timelineinfo_Subject` = @Timelineinfo_Subject 
                                         WHERE (`IdEditHistory` = '{EditHistoryId}');";
            await data.SaveData(updateEditHistory, editHistory, GetConnectionString());
            navManager.NavigateTo("/Profile");
            SavingData = false;
        }
    }
}
