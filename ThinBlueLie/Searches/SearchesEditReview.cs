using AutoMapper;
using Dapper;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.DataModels;
using DataAccessLibrary.DataModels.EditModels;
using DataAccessLibrary.UserModels;
using Microsoft.AspNetCore.Components;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ThinBlueLie.Bases;
using ThinBlueLie.Helper;
using ThinBlueLie.Models;
using ThinBlueLie.Models.View_Models;
using ThinBlueLie.ViewModels;
using static DataAccessLibrary.Enums.EditEnums;
using static ThinBlueLie.Helper.ConfigHelper;
using static ThinBlueLie.Models.View_Models.EditReviewModel;
using static ThinBlueLie.Searches.SearchClasses;

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
        public async Task<List<FirstLoadEditHistory>> GetPendingEdits(ApplicationUser user, bool canReviewAll = false)
        {
            string getPendingEditsIdsSql;
            IEnumerable<FirstLoadEditHistory> PendingIds;
            using (var connection = new MySqlConnection(GetConnectionString()))
            {
                if (canReviewAll)
                {
                    //see what and if the person voted on
                    //if null or 0 nothing else add active to buttons
                    getPendingEditsIdsSql = @"SELECT e.IdEditHistory, e.IdTimelineinfo, ev.Vote 
                                            FROM edithistory e
                                            LEFT Join edit_votes ev on e.IdEditHistory = ev.IdEditHistory And ev.UserId = @UserId 
                                            WHERE Confirmed = 0;";
                    PendingIds = await connection.QueryAsync<FirstLoadEditHistory>(getPendingEditsIdsSql, new {UserId = user.Id});                   
                }
                else
                {
                    getPendingEditsIdsSql = @"SELECT e.IdEditHistory, e.IdTimelineinfo, ev.Vote 
                                             FROM edithistory e 
                                             LEFT JOIN timelineinfo t ON e.IdTimelineinfo = t.IdTimelineinfo 
                                             LEFT JOIN edit_votes ev ON e.IdEditHistory = ev.IdEditHistory And ev.UserId = @UserId 
                                             WHERE e.Confirmed = 0 
                                             AND (t.Owner = @UserId OR e.IdTimelineinfo is null);";
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
                string SubjectChangedQuery = "Select s.IdSubject, s.Name, s.Race, s.Sex From edits_subject s Where s.IdEditHistory = @id";
                Subjects subjectChanges = await data.LoadDataSingle<Subjects, dynamic>(SubjectChangedQuery, new { id }, GetConnectionString());
                people.New.SubjectPerson = subjectChanges; //Get new subject

                string SubjectOldQuery = "Select * From subjects s Where s.IdSubject = @id;";                
                var subjectsOld = await data.LoadDataSingle<Subjects, dynamic>(SubjectOldQuery, new { id = subjectChanges.IdSubject}, GetConnectionString());
                people.Old.SubjectPerson = subjectsOld;
                
            }
            if (editChanges.Officers == 1)
            {
                string OfficerChangedQuery = "Select s.IdOfficer, s.Name, s.Race, s.Sex From edits_officer s Where s.IdEditHistory = @id";
                var officerChanges = await data.LoadDataSingle<Officers, dynamic>(OfficerChangedQuery, new { id }, GetConnectionString());
                people.New.OfficerPerson = officerChanges; //Get new officer

                string OfficerOldQuery = "Select * From officers s Where s.IdOfficer = @id;";
                var officersOld = await data.LoadDataSingle<Officers, dynamic>(OfficerOldQuery, new { id = officerChanges.IdOfficer }, GetConnectionString());
                people.Old.OfficerPerson = officersOld;
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
                List<Media> media = await data.LoadData<Media, dynamic>(mediaQuery, new { id }, GetConnectionString());
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
                        var media = Mapper.Map<EditMedia, Media>(change);
                        newInfo.Medias.Add(media);
                    }
                    else if (action == EditActions.Update)
                    {
                        var media = Mapper.Map<EditMedia, Media>(change);
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
                    @"SELECT o.IdOfficer, o.Name, o.Race, o.Sex, t_o.Age, t_o.Misconduct, t_o.Weapon 
                        FROM edithistory e
                        JOIN edits_timelineinfo_officer t_o ON e.IdEditHistory = t_o.IdEditHistory 
                        JOIN edits_officer o ON t_o.IdEditsOfficer = o.IdEditsOfficer 
                        WHERE e.IdEditHistory = @id;";
                var changesToTimelineOfficer = await data.LoadData<DBOfficer, dynamic>(changesToTimelineOfficerQuery, new { id = id.IdEditHistory }, GetConnectionString());
                newInfo.Officers = changesToTimelineOfficer;
            }
            if (editChanges.Timelineinfo_Subject == 1)
            {
                string changesToTimelineSubjectQuery =
                   @"SELECT s.IdSubject, s.Name, s.Race, s.Sex, t_o.Age, t_o.Misconduct, t_o.Weapon 
                        FROM edithistory e
                        JOIN edits_timelineinfo_subject t_o ON e.IdEditHistory = t_o.IdEditHistory 
                        JOIN edits_subject s ON t_o.IdEditsSubject = o.IdEditsSubject 
                        WHERE e.IdEditHistory = @id;";
                var changesToTimelineSubject = await data.LoadData<DBSubject, dynamic>(changesToTimelineSubjectQuery, new { id = id.IdEditHistory }, GetConnectionString());
                newInfo.Subjects = changesToTimelineSubject;
            }           
            return new Tuple<EditReviewSegment, EditHistory>( newInfo, editChanges);
        }
    }
}
