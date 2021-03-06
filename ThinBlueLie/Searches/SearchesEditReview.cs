using AutoMapper;
using Dapper;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.DataModels;
using DataAccessLibrary.DataModels.EditModels;
using DataAccessLibrary.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DataAccessLibrary.Enums.EditEnums;
using static ThinBlueLie.Helper.ConfigHelper;
using static ThinBlueLie.Searches.SearchClasses;
using static ThinBlueLie.Helper.Extensions.IdentityExtensions;
using DataAccessLibrary.Enums;
using static ThinBlueLie.Helper.Extensions.IntExtensions;
using ThinBlueLie.Models;
using ThinBlueLie.Models.ViewModels;
using static ThinBlueLie.Models.ViewModels.EditReviewModel;
using MySqlConnector;

namespace ThinBlueLie.Searches
{
    public class SearchesEditReview
    {
        public SearchesEditReview(IMapper mapper, IDataAccess data)
        {
            Mapper = mapper;
            Data = data;
        }
        public IDataAccess Data { get; }
        private IMapper Mapper { get; }

        //count total pending edits that the user can access and get their edithistory ids
        public async Task<List<FirstLoadEditHistory>> GetPendingEdits(ApplicationUser user)
        {
            var canReviewAll = user.RepAuthorizer(PrivilegeEnum.Privileges.ReviewAll);
            using (var connection = new MySqlConnection(GetConnectionString()))
            {
                IEnumerable<FirstLoadEditHistory> PendingIds;
                string getPendingEditsIdsSql;
                if (canReviewAll)
                {
                    //If idTimelineinfo does not exist in timelineinfo, it's a new event. 
                    getPendingEditsIdsSql =
                        "SELECT e.IdEditHistory, e.IdTimelineinfo, ev.Vote, e.SubmittedBy, (t.IdTimelineinfo is null and e.Edits != 0) as IsNewEvent " +
                        "FROM edithistory e " +
                        "Left Join timelineinfo t on e.IdTimelineinfo = t.IdTimelineinfo " +
                        "LEFT Join edit_votes ev on e.IdEditHistory = ev.IdEditHistory And ev.UserId = @UserId " +
                        "WHERE Confirmed = 0 " +
                        "And ev.Vote is null " +
                        "And e.SubmittedBy != @UserId;";
                    PendingIds = await connection.QueryAsync<FirstLoadEditHistory>(getPendingEditsIdsSql, new {UserId = user.Id});
                }
                else
                {
                    getPendingEditsIdsSql =
                        "SELECT e.IdEditHistory, e.IdTimelineinfo, ev.Vote, e.SubmittedBy, (t.IdTimelineinfo is null and e.Edits != 0) as IsNewEvent " +
                        "FROM edithistory e " +
                        "LEFT JOIN timelineinfo t ON e.IdTimelineinfo = t.IdTimelineinfo " +
                        "LEFT JOIN edit_votes ev ON e.IdEditHistory = ev.IdEditHistory And ev.UserId = @UserId " +
                        "WHERE e.Confirmed = 0 " +
                        "And e.SubmittedBy != @UserId " +
                        "And ev.Vote is null " +
                        "AND (t.Owner = @UserId OR e.IdTimelineinfo is null);";
                    PendingIds =
                        await connection.QueryAsync<FirstLoadEditHistory>(getPendingEditsIdsSql,
                            new {UserId = user.Id});
                }

                return (List<FirstLoadEditHistory>) PendingIds;
            }
        }

        public async Task<Tuple<EditReviewModel, EditHistory>> GetEditFromId(FirstLoadEditHistory Ids)
        {
            EditReviewSegment OldInfo;
            EditReviewSegment NewInfo;
            EditHistory editHistory;
            if (Ids.IdTimelineinfo != null)
            {
                OldInfo = await GetOldInfoFromId((int) Ids.IdTimelineinfo);
                var newInfo = await GetNewInfoFromId(Ids, OldInfo);
                NewInfo = newInfo.Item1;
                editHistory = newInfo.Item2;
            }
            else
            {
                var peopleChanges = await GetPeopleChangesFromId(Ids.IdEditHistory);
                return peopleChanges;
            }

            var model = new EditReviewModel {Old = OldInfo, New = NewInfo};
            return new Tuple<EditReviewModel, EditHistory>(model, editHistory);
        }

        private async Task<Tuple<EditReviewModel, EditHistory>> GetPeopleChangesFromId(int id)
        {
            var people = new EditReviewModel {New = new EditReviewSegment(), Old = new EditReviewSegment()};
            var WhatChangedQuery = "Select * from edithistory e Where e.IdEditHistory = @id";
            var editChanges =
                await Data.LoadDataSingle<EditHistory, dynamic>(WhatChangedQuery, new {id});
            if (editChanges.Subjects == 1)
            {
                var SubjectChangedQuery = "Select * From edits_subject s Where s.IdEditHistory = @id";
                people.New.SubjectPerson =
                    await Data.LoadDataSingle<SimilarSubject, dynamic>(SubjectChangedQuery, new {id}); //Get new subject

                var SubjectOldQuery = "Select * From subjects s Where s.IdSubject = @id;";
                people.Old.SubjectPerson = await Data.LoadDataSingle<SimilarSubject, dynamic>(SubjectOldQuery,
                    new {id = people.New.SubjectPerson.IdSubject});

                var sql3 = "SELECT t.IdTimelineinfo, t.Date, t.City, t.State " +
                           "FROM timelineinfo t " +
                           "JOIN timelineinfo_subject ON t.IdTimelineinfo = timelineinfo_subject.IdTimelineinfo " +
                           "JOIN subjects s ON timelineinfo_subject.IdSubject = s.IdSubject " +
                           "WHERE s.IdSubject = @id;";
                people.New.SubjectPerson.Events = people.Old.SubjectPerson.Events
                    = await Data.LoadData<SimilarPerson.SimilarPersonEvents, dynamic>(sql3,
                        new {id = people.New.SubjectPerson.IdSubject});
            }

            if (editChanges.Officers == 1)
            {
                var OfficerChangedQuery = "Select * From edits_officer s Where s.IdEditHistory = @id";
                people.New.OfficerPerson =
                    await Data.LoadDataSingle<SimilarOfficer, dynamic>(OfficerChangedQuery, new {id});

                var OfficerOldQuery = "Select * From officers s Where s.IdOfficer = @id;";
                people.Old.OfficerPerson = await Data.LoadDataSingle<SimilarOfficer, dynamic>(OfficerOldQuery,
                    new {id = people.New.OfficerPerson.IdOfficer});

                var sql3 = "SELECT t.IdTimelineinfo, t.Date, t.City, t.State " +
                           "FROM timelineinfo t " +
                           "JOIN timelineinfo_officer ON t.IdTimelineinfo = timelineinfo_officer.IdTimelineinfo " +
                           "JOIN officers o ON timelineinfo_officer.IdOfficer = o.IdOfficer " +
                           "WHERE o.IdOfficer = @id;";
                people.New.OfficerPerson.Events =
                    await Data.LoadData<SimilarPerson.SimilarPersonEvents, dynamic>(sql3,
                        new {id = people.New.OfficerPerson.IdOfficer});
            }

            return new Tuple<EditReviewModel, EditHistory>(people, editChanges);
        }

        public async Task<EditReviewSegment> GetOldInfoFromId(int id)
        {
            //for edits reference edit w/ id that matches id against active timelineinfo with matching idtimelineinfo
            //for editmedia reference reference against active and use action to determine what to do
            //clearing previous information
            //get old information from DB
            var query = "SELECT * From timelineinfo t where t.IdTimelineinfo = @id";
            var timelineinfo = await Data.LoadDataSingle<Timelineinfo, dynamic>(query, new {id});
            var mediaQuery = "SELECT *, (true) as Processed From media m where m.IdTimelineinfo = @id Order By m.Rank;";
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
            var media = await Data.LoadData<ViewMedia, dynamic>(mediaQuery, new {id});
            var officers = await Data.LoadData<DBOfficer, dynamic>(officerQuery, new {id});
            var subjects = await Data.LoadData<DBSubject, dynamic>(subjectQuery, new {id});

            media = await ViewMedia.GetDataMany(media);

            return new EditReviewSegment
            {
                Data = timelineinfo,
                Medias = media,
                Officers = officers,
                Subjects = subjects
            };
        }

        public async Task<Tuple<EditReviewSegment, EditHistory>> GetNewInfoFromId(FirstLoadEditHistory id,
            EditReviewSegment oldInfo)
        {
            var newInfo = new EditReviewSegment();
            Mapper.Map(oldInfo, newInfo);
            var WhatChangedQuery = "Select * from edithistory e Where e.IdEditHistory = @id";
            var editChanges = await Data.LoadDataSingle<EditHistory, dynamic>(WhatChangedQuery,
                new {id = id.IdEditHistory});
            //If Timelineinfo changed
            //TODO move to querymultipleasync
            if (editChanges.Edits == 1)
            {
                var EditChangedQuery = @"Select e.IdTimelineinfo, e.Title, e.Date, e.State, e.City, e.Context, e.Locked 
                                        From edits e Where e.IdEditHistory = @id;";
                newInfo.Data = await Data.LoadDataSingle<Timelineinfo, dynamic>(EditChangedQuery,
                    new {id = id.IdEditHistory});
            }

            if (editChanges.EditMedia == 1)
            {
                var MediaChangedQuery = @"Select *, (true) as Processed
                                          From editmedia m Where m.IdEditHistory = @id Order By m.Rank;";
                var changesToMedia = await Data.LoadData<EditMedia, dynamic>(MediaChangedQuery,
                    new {id = id.IdEditHistory});
                //the frist media isn't getting it's set called for some reason, and thus its urls are empty
                foreach (var change in changesToMedia)
                {
                    var action = (EditActions) change.Action;

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

                await ViewMedia.GetDataMany(newInfo.Medias);
            }

            if (editChanges.Timelineinfo_Officer == 1)
            {
                var changesToTimelineOfficerQuery =
                    $@"SELECT o.*, t_o.Age, t_o.Misconduct, t_o.Weapon 
                        FROM edithistory e
                        JOIN edits_timelineinfo_officer t_o ON e.IdEditHistory = t_o.IdEditHistory 
                        JOIN {(id.IsNewEvent.ToBool() ? "edits_officer" : "officers")} o ON t_o.IdOfficer = o.IdOfficer 
                        WHERE e.IdEditHistory = {id.IdEditHistory};";
                var changesToTimelineOfficer =
                    await Data.LoadData<DBOfficer, dynamic>(changesToTimelineOfficerQuery, new { });
                newInfo.Officers = changesToTimelineOfficer;
            }

            if (editChanges.Timelineinfo_Subject == 1)
            {
                var changesToTimelineSubjectQuery =
                    $@"SELECT s.*, t_s.Age, t_s.Armed
                        FROM edithistory e
                        JOIN edits_timelineinfo_subject t_s ON e.IdEditHistory = t_s.IdEditHistory 
                        JOIN {(id.IsNewEvent.ToBool() ? "edits_subject" : "subjects")} s ON t_s.IdSubject = s.IdSubject 
                        WHERE e.IdEditHistory = @id;";
                var changesToTimelineSubject = await Data.LoadData<DBSubject, dynamic>(changesToTimelineSubjectQuery,
                    new {id = id.IdEditHistory});
                newInfo.Subjects = changesToTimelineSubject;
            }

            return new Tuple<EditReviewSegment, EditHistory>(newInfo, editChanges);
        }
    }
}