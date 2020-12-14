using AutoMapper;
using Dapper;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.DataModels;
using DataAccessLibrary.DataModels.EditModels;
using DataAccessLibrary.UserModels;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLie.Models.View_Models;
using static DataAccessLibrary.Enums.EditEnums;
using static ThinBlueLie.Helper.ConfigHelper;
using static ThinBlueLie.Models.View_Models.EditReviewModel;
using static ThinBlueLie.Searches.SearchClasses;
using static ThinBlueLie.Helper.Extensions.IdentityExtensions;
using DataAccessLibrary.Enums;
using static ThinBlueLie.Helper.Extensions.IntExtensions;
using ThinBlueLie.Models;

namespace ThinBlueLie.Searches
{
    public class SearchesEditReview
    {
        public SearchesEditReview(IMapper mapper)
        {
            this.Mapper = mapper;
        }

        IMapper Mapper { get; set; }

        //count total pending edits that the user can access and get their edithistory ids
        public async Task<List<FirstLoadEditHistory>> GetPendingEdits(ApplicationUser user)
        {
            string getPendingEditsIdsSql;
            bool canReviewAll = user.RepAuthorizer(PrivledgeEnum.Privledges.ReviewAll);
            IEnumerable<FirstLoadEditHistory> PendingIds;
            using (var connection = new MySqlConnection(GetConnectionString()))
            {
                if (canReviewAll)
                {
                    //If idTimelineinfo does not exist in timelineinfo, it's a new event. 
                    getPendingEditsIdsSql = "SELECT e.IdEditHistory, e.IdTimelineinfo, ev.Vote, e.SubmittedBy, (t.IdTimelineinfo is null and e.Edits != 0) as IsNewEvent " +
                                                "FROM edithistory e " +
                                                "Left Join timelineinfo t on e.IdTimelineinfo = t.IdTimelineinfo " +
                                                "LEFT Join edit_votes ev on e.IdEditHistory = ev.IdEditHistory And ev.UserId = @UserId " +
                                                "WHERE Confirmed = 0;";
                                                //"And e.SubmittedBy != @UserId;";
                    PendingIds = await connection.QueryAsync<FirstLoadEditHistory>(getPendingEditsIdsSql, new {UserId = user.Id});                   
                }
                else
                {
                    getPendingEditsIdsSql = "SELECT e.IdEditHistory, e.IdTimelineinfo, ev.Vote, e.SubmittedBy, (t.IdTimelineinfo is null and e.Edits != 0) as IsNewEvent " +
                                                 "FROM edithistory e " +
                                                 "LEFT JOIN timelineinfo t ON e.IdTimelineinfo = t.IdTimelineinfo " +
                                                 "LEFT JOIN edit_votes ev ON e.IdEditHistory = ev.IdEditHistory And ev.UserId = @UserId " +
                                                 "WHERE e.Confirmed = 0 " +
                                                 "And e.SubmittedBy != @UserId" +
                                                 "AND (t.Owner = @UserId OR e.IdTimelineinfo is null);";
                    PendingIds = await connection.QueryAsync<FirstLoadEditHistory>(getPendingEditsIdsSql, new {UserId = user.Id});                  
                }
                return (List<FirstLoadEditHistory>)PendingIds;
            }
        }

        public async Task<Tuple<EditReviewModel, EditHistory>> GetEditFromId(FirstLoadEditHistory Ids)
        {
            EditReviewSegment OldInfo;
            EditReviewSegment NewInfo;
            EditHistory editHistory;
            if (Ids.IdTimelineinfo != null)
            {
                OldInfo = await GetOldInfoFromId((int)Ids.IdTimelineinfo);
                var newInfo = await GetNewInfoFromId(Ids, OldInfo);
                NewInfo = newInfo.Item1;
                editHistory = newInfo.Item2;
            }
            else
            {
                var peopleChanges = await GetPeopleChangesFromId(Ids.IdEditHistory);
                return peopleChanges;
            }
            var model = new EditReviewModel { Old = OldInfo, New = NewInfo };
            return new Tuple<EditReviewModel, EditHistory>(model, editHistory);
        }

        async Task<Tuple<EditReviewModel, EditHistory>> GetPeopleChangesFromId(int id)
        {
            EditReviewModel people = new EditReviewModel {New = new EditReviewSegment(), Old = new EditReviewSegment() };
            DataAccess data = new DataAccess();
            var WhatChangedQuery = "Select * from edithistory e Where e.IdEditHistory = @id";
            EditHistory editChanges = await data.LoadDataSingle<EditHistory, dynamic>(WhatChangedQuery, new { id }, GetConnectionString());
            if (editChanges.Subjects == 1)
            {
                string SubjectChangedQuery = "Select * From edits_subject s Where s.IdEditHistory = @id";
                people.New.SubjectPerson = await data.LoadDataSingle<SimilarSubject, dynamic>(SubjectChangedQuery, new { id }, GetConnectionString()); //Get new subject

                string SubjectOldQuery = "Select * From subjects s Where s.IdSubject = @id;";
                people.Old.SubjectPerson = await data.LoadDataSingle<SimilarSubject, dynamic>(SubjectOldQuery, new { id = people.New.SubjectPerson.IdSubject}, GetConnectionString());

                var sql3 = "SELECT t.IdTimelineinfo, t.Date, t.City, t.State " +
                                "FROM timelineinfo t " +
                                "JOIN timelineinfo_subject ON t.IdTimelineinfo = timelineinfo_subject.IdTimelineinfo " +
                                "JOIN subjects s ON timelineinfo_subject.IdSubject = s.IdSubject " +
                                "WHERE s.IdSubject = @id;";
                people.New.SubjectPerson.Events = people.Old.SubjectPerson.Events
                    = await data.LoadData<SimilarPerson.SimilarPersonEvents, dynamic>(sql3, new { id = people.New.SubjectPerson.IdSubject }, GetConnectionString());
            }
            if (editChanges.Officers == 1)
            {   
                string OfficerChangedQuery = "Select * From edits_officer s Where s.IdEditHistory = @id";
                people.New.OfficerPerson = await data.LoadDataSingle<SimilarOfficer, dynamic>(OfficerChangedQuery, new { id }, GetConnectionString());

                string OfficerOldQuery = "Select * From officers s Where s.IdOfficer = @id;";
                people.Old.OfficerPerson = await data.LoadDataSingle<SimilarOfficer, dynamic>(OfficerOldQuery, new { id = people.New.OfficerPerson.IdOfficer }, GetConnectionString());

                var sql3 = "SELECT t.IdTimelineinfo, t.Date, t.City, t.State " +
                              "FROM timelineinfo t " +
                              "JOIN timelineinfo_officer ON t.IdTimelineinfo = timelineinfo_officer.IdTimelineinfo " +
                              "JOIN officers o ON timelineinfo_officer.IdOfficer = o.IdOfficer " +
                              "WHERE o.IdOfficer = @id;";
                people.New.OfficerPerson.Events = people.Old.OfficerPerson.Events =
                    await data.LoadData<SimilarPerson.SimilarPersonEvents, dynamic>(sql3, new { id = people.New.OfficerPerson.IdOfficer }, GetConnectionString());
            }

            return new Tuple<EditReviewModel, EditHistory>( people, editChanges);
        }
        public async Task<EditReviewSegment> GetOldInfoFromId(int id)
        {
            //for edits reference edit w/ id that matches id against active timelineinfo with matching idtimelineinfo
            //for editmedia reference reference against active and use action to determine what to do
            //clearing previous information
            DataAccess data = new DataAccess();

            //get old information from DB
            string query = "SELECT * From timelineinfo t where t.IdTimelineinfo = @id";
            Timelineinfo timelineinfo = await data.LoadDataSingle<Timelineinfo, dynamic>(query, new { id }, GetConnectionString());
            string mediaQuery = "SELECT m.IdMedia, m.MediaType, m.SourcePath, m.Gore, m.SourceFrom, m.Blurb, m.Credit, m.SubmittedBy, m.Rank From media m where m.IdTimelineinfo = @id Order By m.Rank;";
            string officerQuery = "SELECT o.IdOfficer, o.Name, o.Race, o.Sex, t_o.Age, t_o.Misconduct, t_o.Weapon " +
                        "FROM timelineinfo t " +
                        "JOIN timelineinfo_officer t_o ON t.IdTimelineinfo = t_o.IdTimelineinfo " +
                        "JOIN officers o ON t_o.IdOfficer = o.IdOfficer " +
                        "WHERE t.IdTimelineinfo = @id ;";
            string subjectQuery = "SELECT s.IdSubject, s.Name, s.Race, s.Sex, t_s.Age, t_s.Armed " +
                        "FROM timelineinfo t " +
                        "JOIN timelineinfo_subject t_s ON t.IdTimelineinfo = t_s.IdTimelineinfo " +
                        "JOIN subjects s ON t_s.IdSubject = s.IdSubject " +
                        "WHERE t.IdTimelineinfo = @id;";

                //get media, officers, and subjects using timelineinfo id
                List<ViewMedia> media = await data.LoadData<ViewMedia, dynamic>(mediaQuery, new { id }, GetConnectionString());
                List<DBOfficer> officers = await data.LoadData<DBOfficer, dynamic>(officerQuery, new { id }, GetConnectionString());
                List<DBSubject> subjects = await data.LoadData<DBSubject, dynamic>(subjectQuery, new { id }, GetConnectionString());

                return new EditReviewSegment
                {
                    Data = timelineinfo,
                    Medias = media,
                    Officers = officers,
                    Subjects = subjects
                };            
        }

        public async Task<Tuple<EditReviewSegment, EditHistory>> GetNewInfoFromId(FirstLoadEditHistory id, EditReviewSegment oldInfo)
        {
            EditReviewSegment newInfo = new EditReviewSegment();
            Mapper.Map(oldInfo, newInfo);
            DataAccess data = new DataAccess();
            string WhatChangedQuery = "Select * from edithistory e Where e.IdEditHistory = @id";
            EditHistory editChanges = await data.LoadDataSingle<EditHistory, dynamic>(WhatChangedQuery, new {id = id.IdEditHistory}, GetConnectionString());
            //If Timelineinfo changed
            //TODO move to querymultipleasync
            if (editChanges.Edits == 1)
            {
                string EditChangedQuery = @"Select e.IdTimelineinfo, e.Title, e.Date, e.State, e.City, e.Context, e.Locked 
                                        From edits e Where e.IdEditHistory = @id;";
                newInfo.Data = await data.LoadDataSingle<Timelineinfo, dynamic>(EditChangedQuery, new { id = id.IdEditHistory }, GetConnectionString());
            }
            if (editChanges.EditMedia == 1)
            {
                string MediaChangedQuery = @"Select *
                                          From editmedia m Where m.IdEditHistory = @id;";
                var changesToMedia = await data.LoadData<EditMedia, dynamic>(MediaChangedQuery, new { id = id.IdEditHistory }, GetConnectionString());
                foreach (var change in changesToMedia)
                {
                    var action = (EditActions)change.Action;

                    if (action == EditActions.Addition)
                    {
                        var media = Mapper.Map<EditMedia, ViewMedia>(change);
                        newInfo.Medias.Add(media);
                    }
                    else if (action == EditActions.Update)
                    {
                        var media = Mapper.Map<EditMedia, ViewMedia>(change);
                        newInfo.Medias.Where(m => m.IdMedia == change.IdMedia).ToList().ForEach(m => m = media);
                    }
                    else if (action == EditActions.Deletion)
                    {
                        newInfo.Medias.RemoveAll(m => m.IdMedia == change.IdMedia);
                    }
                }
            }
            if (editChanges.Timelineinfo_Officer == 1)
            {
                string changesToTimelineOfficerQuery = 
                    $@"SELECT o.IdOfficer, o.Name, o.Race, o.Sex, t_o.Age, t_o.Misconduct, t_o.Weapon 
                        FROM edithistory e
                        JOIN edits_timelineinfo_officer t_o ON e.IdEditHistory = t_o.IdEditHistory 
                        JOIN {(id.IsNewEvent.ToBool()? "edits_officer" : "officers")} o ON t_o.IdOfficer = o.IdOfficer 
                        WHERE e.IdEditHistory = {id.IdEditHistory};";
                var changesToTimelineOfficer = await data.LoadData<DBOfficer, dynamic>(changesToTimelineOfficerQuery, new { }, GetConnectionString());
                newInfo.Officers = changesToTimelineOfficer;
            }
            if (editChanges.Timelineinfo_Subject == 1)
            {
                string changesToTimelineSubjectQuery =
                   $@"SELECT s.IdSubject, s.Name, s.Race, s.Sex, t_s.Age, t_s.Armed
                        FROM edithistory e
                        JOIN edits_timelineinfo_subject t_s ON e.IdEditHistory = t_s.IdEditHistory 
                        JOIN {(id.IsNewEvent.ToBool()? "edits_subject" : "subjects")} s ON t_s.IdSubject = s.IdSubject 
                        WHERE e.IdEditHistory = @id;";
                var changesToTimelineSubject = await data.LoadData<DBSubject, dynamic>(changesToTimelineSubjectQuery, new { id = id.IdEditHistory }, GetConnectionString());
                newInfo.Subjects = changesToTimelineSubject;
            }           
            return new Tuple<EditReviewSegment, EditHistory>( newInfo, editChanges);
        }
    }
}
