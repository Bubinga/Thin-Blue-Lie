﻿using DataAccessLibrary.DataAccess;
using DataAccessLibrary.DataModels;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLieB.Models;
using static ThinBlueLieB.Helper.Algorithms;
using static ThinBlueLieB.Models.ViewSimilar;
using static ThinBlueLieB.Searches.SearchClasses;
using static ThinBlueLieB.Helper.ConnectionStringHelper;

namespace ThinBlueLieB.Helper
{
    public class SearchesSubmit
    {      
        public async Task<List<SimilarPersonGeneral>> SearchOfficer(string Input)
        {
            //Get only First and Last Name
            var FirstName = Input.Split(' ').FirstOrDefault();
            var LastName = Input.Split(' ').Last();
            var NormalizedName = FirstName + " " + LastName;
            //TODO for names like: J. Alexander Kueng or make it into J. Kueng and Alexander Kueng and test both
            var MiddleCount1 = Input.Replace(" " + LastName, "").Replace(FirstName, "");           
            string[] MiddleCount2;
            int MiddleCount;
            if (MiddleCount1 != string.Empty)
            {
                MiddleCount2 = MiddleCount1.Split(' ');
                MiddleCount = MiddleCount2.Count();
            }
            else
            {
                MiddleCount = 0;
            }
            

            //Load all OfficcerNames
            DataAccess data = new DataAccess();
            string sql = "SELECT IdOfficer, Name FROM officers";
            List<FirstLoadOfficer> Names = await data.LoadData<FirstLoadOfficer, dynamic>(sql, new { }, GetConnectionString());

            List<NameScore> nameScores = new List<NameScore>();
            foreach (var name in Names)
            {
                var first = name.Name.Split(' ').FirstOrDefault();
                var last = name.Name.Split(' ').Last();
                var normalizedName = first + " " + last;
                var middleCount1 = Input.Replace(" " + first, "").Replace(last, "");
                string[] middleCount2;
                int middleCount;
                if (middleCount1 != string.Empty)
                {
                    middleCount2 = middleCount1.Split(' ');
                    middleCount = middleCount2.Count();
                }
                else
                {
                    middleCount = 0;
                }

                var score = JaroWinklerDistance.proximity(normalizedName, NormalizedName);               
                score += Math.Abs(middleCount - MiddleCount) / 40; //40 is just a random number change it later
                var listitem = new NameScore()
                {
                    Id = name.IdOfficer,
                    //Name = normalizedName,
                    Score = score,
                };
                nameScores.Add(listitem);
            }
            //return list of names in order of lowest to highest score
            var SortedNames = nameScores.OrderByDescending(o => o.Score).ToList();
            var Ids = String.Join(",", SortedNames.Where(s => s.Score > 0.7).Select(x => x.Id));            
            List<SimilarOfficer> similarPeople;
            if (!string.IsNullOrWhiteSpace(Ids))
            {
                var sql2 = "SELECT o.Name, o.Race, o.Sex, IdOfficer FROM officers o Where IdOfficer in (" + Ids + ") ORDER BY field(IdOfficer, " + Ids + ")";
                similarPeople = await data.LoadData<SimilarOfficer, dynamic>(sql2, new { }, GetConnectionString());
                for (int i = 0; i < Ids.Length; i++)
                {
                    var sql3 = "SELECT t.Date, t.City, t.State " +
                                "FROM timelineinfo t " +
                                "JOIN timelineinfo_officer ON t.IdTimelineinfo = timelineinfo_officer.IdTimelineinfo " +
                                "JOIN officers o ON timelineinfo_officer.IdOfficer = o.IdOfficer " +
                                "WHERE o.IdOfficer = " + Ids[i] + ";";
                    similarPeople[i].Events = await data.LoadData<SimilarOfficer.SimilarPersonEvents, dynamic>(sql3, new { }, GetConnectionString());
                }                          
            }
            else
            {
                similarPeople = new List<SimilarOfficer>();
            }
            List<SimilarPersonGeneral> SimilarPeople = new List<SimilarPersonGeneral>();
            foreach (var person in similarPeople)
            {
                var Person = new SimilarPersonGeneral
                {
                    Events = person.Events,
                    Name = person.Name,
                    Race = person.Race,
                    Sex = person.Sex,
                    IdPerson = person.IdOfficer
                };
                SimilarPeople.Add(Person);
            }
            return SimilarPeople;           
        }

        public async Task<List<SimilarPersonGeneral>> SearchSubject(string Input)
        {
            //Get only First and Last Name
            var FirstName = Input.Split(' ').FirstOrDefault();
            var LastName = Input.Split(' ').Last();
            var NormalizedName = FirstName + " " + LastName;
            //TODO for names like: J. Alexander Kueng or make it into J. Kueng and Alexander Kueng and test both
            var MiddleCount1 = Input.Replace(" " + LastName, "").Replace(FirstName, "");
            string[] MiddleCount2;
            int MiddleCount;
            if (MiddleCount1 != string.Empty)
            {
                MiddleCount2 = MiddleCount1.Split(' ');
                MiddleCount = MiddleCount2.Count();
            }
            else
            {
                MiddleCount = 0;
            }


            //Load all OfficcerNames
            DataAccess data = new DataAccess();
            string sql = "SELECT IdSubject, Name FROM subjects";
            List<FirstLoadSubject> Names = await data.LoadData<FirstLoadSubject, dynamic>(sql, new { }, GetConnectionString());

            List<NameScore> nameScores = new List<NameScore>();
            foreach (var name in Names)
            {
                var first = name.Name.Split(' ').FirstOrDefault();
                var last = name.Name.Split(' ').Last();
                var normalizedName = first + " " + last;
                var middleCount1 = Input.Replace(" " + first, "").Replace(last, "");
                string[] middleCount2;
                int middleCount;
                if (middleCount1 != string.Empty)
                {
                    middleCount2 = middleCount1.Split(' ');
                    middleCount = middleCount2.Count();
                }
                else
                {
                    middleCount = 0;
                }

                var score = JaroWinklerDistance.proximity(normalizedName, NormalizedName);
                score += Math.Abs(middleCount - MiddleCount) / 40; //40 is just a random number change it later
                var listitem = new NameScore()
                {
                    Id = name.IdSubject,
                    //Name = normalizedName,
                    Score = score,
                };
                nameScores.Add(listitem);
            }
            //return list of names in order of lowest to highest score
            var SortedNames = nameScores.OrderByDescending(o => o.Score).ToList();
            var Ids = String.Join(",", SortedNames.Where(s => s.Score > 0.7).Select(x => x.Id));           
            List<SimilarSubject> similarPeople;
            if (!string.IsNullOrWhiteSpace(Ids))
            {
                var sql2 = "SELECT * FROM subjects Where IdSubject in (" + Ids + ") ORDER BY field(IdSubject, " + Ids + ")";
                similarPeople = await data.LoadData<SimilarSubject, dynamic>(sql2, new { }, GetConnectionString());
                for (int i = 0; i < Ids.Length; i++)
                {
                    var sql3 = "SELECT t.Date, t.City, t.State " +
                                "FROM timelineinfo t " +
                                "JOIN timelineinfo_subject ON t.IdTimelineinfo = timelineinfo_subject.IdTimelineinfo " +
                                "JOIN subjects s ON timelineinfo_subject.IdSubject = s.IdSubject " +
                                "WHERE s.IdSubject = " + Ids[i] + ";";
                    similarPeople[i].Events = await data.LoadData<SimilarSubject.SimilarPersonEvents, dynamic>(sql3, new { }, GetConnectionString());
                }
            }
            else
            {
                similarPeople = new List<SimilarSubject>();
            }
            List<SimilarPersonGeneral> SimilarPeople = new List<SimilarPersonGeneral>();
            foreach (var person in similarPeople)
            {
                var Person = new SimilarPersonGeneral
                {
                    Events = person.Events,
                    Name = person.Name,
                    Race = person.Race,
                    Sex = person.Sex,
                    IdPerson = person.IdSubject
                };
                SimilarPeople.Add(Person);
            }
            return SimilarPeople;
        }

        public async Task<List<ViewSimilar>> GetSimilar(string? TempDate)
        {
            //load events where date or officer/subject name is shared and load it into SimilarEvents.            
            List<ViewSimilar> SimilarEvents = new List<ViewSimilar>();
            DataAccess data = new DataAccess();
            var query1 = "SELECT t.Date, t.IdTimelineinfo, t.Context, t.State, t.City, t.Verified From timelineinfo t where t.date = " + "'" + TempDate + "'" + ";";
            var SimilarTimelineinfos = await data.LoadData<Timelineinfo, dynamic>(query1, new { }, GetConnectionString());
            foreach ((var Event, Int32 i) in SimilarTimelineinfos.Select((Event, i) => (Event, i)))
            {
                var id = Event.IdTimelineinfo;
                var officerQuery = "SELECT o.Name, o.Race, o.Sex, tio.Age FROM timelineinfo t JOIN timelineinfo_officer tio ON t.IdTimelineinfo = tio.IdTimelineinfo JOIN officers o ON tio.IdOfficer = o.IdOfficer WHERE t.IdTimelineinfo = " + id + ";";
                var Officers = await data.LoadData<ViewSimilarPerson, dynamic>(officerQuery, new { }, GetConnectionString());
                var subjectQuery = "SELECT s.Name, s.Race, s.Sex, ts.Age FROM timelineinfo t JOIN timelineinfo_subject ts ON t.IdTimelineinfo = ts.IdTimelineinfo JOIN subjects s ON ts.IdSubject = s.IdSubject WHERE t.IdTimelineinfo = " + id + ";";
                var Subjects = await data.LoadData<ViewSimilarPerson, dynamic>(subjectQuery, new { }, GetConnectionString());
                var MediaQuery = "Select * From media m Where(m.IdTimelineinfo = " + id + ");";
                var Media = await data.LoadData<Media, dynamic>(officerQuery, new { }, GetConnectionString());

                //add timelineinfos, officers, subject, and media to list at end
                ViewSimilar items = new ViewSimilar()
                {
                    Timelineinfo = Event,
                    Officers = Officers,
                    Subjects = Subjects,
                    Media = Media.FirstOrDefault()
                };
                SimilarEvents.Add(items);
            }
            return SimilarEvents;
        }
    }   
    
}

