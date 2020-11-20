using AutoMapper;
using Dapper;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.DataModels;
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
using ThinBlueLieB.Models;
using ThinBlueLieB.ViewModels;
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

        SubmitModel oldInfo = new SubmitModel();

        public bool EventExists;
        //TODO only add to junction tables is something changes. 
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
            return new SubmitModel{ Timelineinfos = new ViewTimelineinfo()};
        }
        internal AuthenticationState userState;
        internal async void HandleValidSubmitAsync()
        {
            var medias = Mapper.Map<List<ViewMedia>, List<EditMedia>>(model.Medias);
            DataAccess data = new DataAccess();
            //write to 
            // edits
            // editmedia
            int userId = Convert.ToInt32(UserManager.GetUserId(userState.User));
            model.Timelineinfos.SubmittedBy = userId;
            
            var editSql = "INSERT INTO `edits` (`IdTimelineinfo`, `Date`, `State`, `City`, `Context`, `Locked`, `SubmittedBy`, `Confirmed`)" +
                " VALUES (IdTimelineinfo, Date, State, City, Context, Locked, SubmittedBy, Confirmed); " +
                "SELECT LAST_INSERT_ID();";
            int editid = await data.SaveDataAndReturn(editSql, model.Timelineinfos, GetConnectionString());

            var editMediaSql = "INSERT INTO `editmedia` (`IdEdits`, `MediaType`, `SourcePath`, `Gore`, `SourceFrom`, `Blurb`, `Credit`, `SubmittedBy`, `Rank`) " +
                "VALUES (IdEdits, MediaType, SourcePath, Gore, SourceFrom, Blurb, Credit, SubmittedBy, Rank);";
            //var medias = mapper.Map<List<ViewMedia>, List<EditMedia>>(model.Medias);

            for (int i = 0; i < medias.Count; i++)            
            {
                medias[i].IdTimelineinfo = editid;
                //if matches submittedby stays the same, if different use userId
                if (model.Medias[i] == oldInfo.Medias[i]) {
                    medias[i].SubmittedBy = oldInfo.Medias[i].SubmittedBy;
                }
                else {
                    medias[i].SubmittedBy = userId;
                }
                await data.SaveData(editMediaSql, medias[i], GetConnectionString());
            }

            //Subject Table
            foreach (var subject in model.Subjects)
            {
                using (var connection = new MySqlConnection(GetConnectionString()))
                {

                    if (subject.SameAsId == null)
                    {
                        //Create new subject
                        var subjectSql = "INSERT INTO edit_subjects (`Name`, `Race`, `Sex`) " +
                                     "VALUES (@Name, @Race, @Sex);" +
                                     "SELECT LAST_INSERT_ID();";
                        //Add to subjects table and return id
                        var IdSubject = connection.QuerySingle<int>(subjectSql, subject);

                        //Add to junction table
                        var junctionSql = "INSERT INTO edit_timelineinfo_subject (`IdTimelineinfo`, `IdSubject`, `Armed`, `Age`)" +
                                          "VALUES (@idtimelineinfo, @idsubject, @armed, @age);";
                        connection.Execute(junctionSql, new
                        {
                            idtimelineinfo = editid,
                            idsubject = IdSubject,
                            armed = Convert.ToByte(subject.Armed),
                            age = subject.Age
                        });
                    }
                    else
                    {
                        var junctionSql = "INSERT INTO edit_timelineinfo_subject (`IdTimelineinfo`, `IdSubject`, `Armed`, `Age`)" +
                                         "VALUES (@idtimelineinfo, @idsubject, @armed, @age);";
                        connection.Execute(junctionSql, new
                        {
                            idtimelineinfo = editid,
                            idsubject = subject.SameAsId,
                            armed = Convert.ToByte(subject.Armed),
                            age = (byte?)subject.Age
                        });
                    }
                }
            }
            //Officer Table
            foreach (var officer in model.Officers)
            {
                using (var connection = new MySqlConnection(ConfigHelper.GetConnectionString()))
                {

                    if (officer.SameAsId == null)
                    {
                        //Create new officer
                        var officerSql = "INSERT INTO edit_officers (`Name`, `Race`, `Sex`)" +
                                     "VALUES (@Name, @Race, @Sex);" +
                                     "SELECT LAST_INSERT_ID();";
                        //Add to officers table and return id
                        var IdOfficer = connection.QuerySingle<int>(officerSql,officer);

                        //Add to junction table
                        var junctionSql = "INSERT INTO edit_timelineinfo_officer (`IdTimelineinfo`, `IdOfficer`, `Age`, `Misconduct`, `Weapon`)" +
                                        "VALUES (@idtimelineinfo, @idofficer, @age, @misconduct, @weapon);";
                        connection.Execute(junctionSql, new
                        {
                            idtimelineinfo = editid,
                            idofficer = IdOfficer,
                            age = (byte)officer.Age,
                            misconduct = officer.Misconduct.Sum(),
                            weapon = (int)officer.Weapon.Sum(),
                        });
                    }
                    else
                    {
                        var junctionSql = "INSERT INTO edit_timelineinfo_officer (`IdTimelineinfo`, `IdOfficer`, `Age`, `Misconduct`, `Weapon`)" +
                                         "VALUES (@idtimelineinfo, @idofficer, @age, @misconduct, @weapon);";
                        connection.Execute(junctionSql, new
                        {
                            idtimelineinfo = editid,
                            idofficer = officer.SameAsId,
                            age = (byte?)officer.Age,
                            misconduct = officer.Misconduct.Sum(),
                            weapon = officer.Weapon.Sum(),
                        });
                    }
                }
            }
          
        }
    }
}
