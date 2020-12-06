using Dapper;
using DataAccessLibrary.DataModels;
using DataAccessLibrary.UserModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLie.Models;
using static ThinBlueLie.Helper.ConfigHelper;

namespace ThinBlueLie.Bases
{
    public class ProfileBase : ComponentBase
    {
        public ProfileModel profile = new ProfileModel();
        public List<Timelineinfo> Events { get; set; }
        [CascadingParameter]
        public Task<AuthenticationState> AuthState { get; set; }
        public AuthenticationState userState;
        public ApplicationUser User;
        public async Task GetProfile()
        {
            string sql = $@"Select Distinct e.IdTimelineinfo, t.City, t.State, t.Updated, e.Timestamp, t.Owner, t.Title, t.Date
                               From edithistory e
                               Join timelineinfo t on e.IdTimelineinfo = t.IdTimelineinfo
                               Where SubmittedBy = {User.Id};
                            Select Count(*) From edithistory e where e.SubmittedBy = 1 and Confirmed = {User.Id};
                            Select Count(*) From flags f where f.UserId = {User.Id};
                            Select Count(*) From edit_votes ev where ev.UserId = 1;";
            using (var connection = new MySqlConnection(GetConnectionString()))
            {
                connection.Open();

                using (var multi = await connection.QueryMultipleAsync(sql))
                {
                    profile.Submissions = multi.Read<Timelineinfo>().ToList();
                    profile.AcceptedEdits = multi.Read<int>().FirstOrDefault();
                    profile.Flags = multi.Read<int>().FirstOrDefault();
                    profile.VotesCast = multi.Read<int>().FirstOrDefault();
                }
            }
        }

        
        //Accepted Edits: Count(edithistory where userId = userId and Confirmed = 1)
        //Flags: Count(flags where UserId = userid)
        //Submissions: select Count(distinct IdTimelineinfo from edithistory Where userId = userId)
        //    Gets submissions that have even been turned public
        // Votes Cast: select Count(editvotes where userId = userId);

        //Get all Events they Own/Owned select distinct IdTimelineinfo e.IdTimelineinfo from edithistory e Where userId = userId
        //    Date Uploaded
        //    Date of Event
        //    Title
        //    Thoroughness Index (later)
        //    Community
        //
    }
}
