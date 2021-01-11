﻿using Dapper;
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
            string sql = "SELECT " +
                              "e.IdTimelineinfo, t.City, t.State, t.Updated, e.Timestamp, t.Owner, t.Title, t.Date, e.Confirmed as Status, " +
                                "CASE " +
                                  "WHEN  " +
                                    "ROW_NUMBER() OVER (PARTITION BY e.IdTimelineinfo ORDER BY e.Timestamp) = 1 " +
                                    "THEN 1 " +
                                  "ELSE 0 " +
                                "END AS IsNewEvent, " +
                              "ROW_NUMBER() OVER (PARTITION BY IdTimelineinfo ORDER BY e.Timestamp) AS rn " +
                            "From edithistory e " +
                            "Join timelineinfo t on e.IdTimelineinfo = t.IdTimelineinfo " +
                            $"Where SubmittedBy = {User.Id} " +
                            "ORDER BY e.IdTimelineinfo; " +
                        $"Select Count(*) From edithistory e where e.SubmittedBy = 1 and Confirmed = {User.Id}; " +
                        $"Select Count(*) From flags f where f.UserId = {User.Id}; " +
                        $"Select Count(*) From edit_votes ev where ev.UserId = {User.Id};";
            using (var connection = new MySqlConnection(GetConnectionString()))
            {
                connection.Open();

                using (var multi = await connection.QueryMultipleAsync(sql))
                {
                    profile.Submissions = multi.Read<ProfileInfo>().ToList();
                    profile.AcceptedEdits = multi.Read<int>().FirstOrDefault();
                    profile.Flags = multi.Read<int>().FirstOrDefault();
                    profile.VotesCast = multi.Read<int>().FirstOrDefault();
                }
            }
        }
    }
}
