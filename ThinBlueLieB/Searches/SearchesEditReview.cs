using AutoMapper;
using Dapper;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.DataModels;
using DataAccessLibrary.UserModels;
using Microsoft.AspNetCore.Components;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ThinBlueLieB.Bases;
using ThinBlueLieB.Helper;
using ThinBlueLieB.Models;
using ThinBlueLieB.ViewModels;
using static ThinBlueLieB.Helper.ConfigHelper;
using static ThinBlueLieB.Searches.SearchClasses;

namespace ThinBlueLieB.Searches
{
    public class SearchesEditReview
    {
        public SearchesEditReview(IMapper mapper)
        {
            this.mapper = mapper;
        }

        IMapper mapper { get; set; }
        //[Inject]
        //public IMapper mapper { get; set; }
        //count total pending edits that the user can access and get their edithistory ids
        public async Task<List<FirstLoadEditHistory>> GetPendingEdits(ApplicationUser user, bool canReviewAll = false)
        {
            string getPendingEditsIdsSql;
            IEnumerable<FirstLoadEditHistory> PendingIds;
            using (var connection = new MySqlConnection(ConfigHelper.GetConnectionString()))
            {
                if (canReviewAll)
                {
                    //no need to join
                    getPendingEditsIdsSql = "SELECT e.IdEditHistory, e.IdTimelineinfo FROM edithistory e WHERE Confirmed = 0;";
                    PendingIds = await connection.QueryAsync<FirstLoadEditHistory>(getPendingEditsIdsSql);                   
                }
                else
                {
                    getPendingEditsIdsSql = "SELECT e.IdEditHistory, e.IdTimelineinfo FROM edithistory e JOIN timelineinfo t ON e.IdTimelineinfo = t.IdTimelineinfo WHERE e.Confirmed = 0 AND t.Owner = @UserId;";
                    PendingIds = await connection.QueryAsync<FirstLoadEditHistory>(getPendingEditsIdsSql, new {UserId = user.Id});                  
                }
                return (List<FirstLoadEditHistory>)PendingIds;
            }
        }

        public async Task<ViewEvent> GetEditFromId(int id)
        {
            //for edits reference edit w/ id that matches id against active timelineinfo with matching idtimelineinfo
            //for editmedia reference reference against active and use action to determine what to do
            //clearing previous information
            DataAccess data = new DataAccess();

            //get new information from DB
            var query = "SELECT * From timelineinfo t where t.IdTimelineinfo = @id";
            List<Timelineinfo> timelineinfo = await data.LoadData<Timelineinfo, dynamic>(query, new { id = id }, GetConnectionString());  
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
                List<DisplayMedia> media = await data.LoadData<DisplayMedia, dynamic>(mediaQuery, new { id = id }, GetConnectionString());
                List<DBOfficer> officers = await data.LoadData<DBOfficer, dynamic>(officerQuery, new { id = id }, GetConnectionString());
                List<DBSubject> subjects = await data.LoadData<DBSubject, dynamic>(subjectQuery, new { id = id }, GetConnectionString());

                return new ViewEvent
                {
                    Data = timelineinfo.FirstOrDefault(),
                    Medias = media,
                    Officers = mapper.Map<List<DBOfficer>, List<ViewOfficer>>(officers),
                    Subjects = mapper.Map<List<DBSubject>, List<ViewSubject>>(subjects)
                };            
        }
    }
}
