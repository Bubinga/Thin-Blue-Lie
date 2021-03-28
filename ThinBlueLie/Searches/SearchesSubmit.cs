using DataAccessLibrary.DataAccess;
using DataAccessLibrary.DataModels;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLie.Models;
using static ThinBlueLie.Models.ViewSimilar;
using static ThinBlueLie.Searches.SearchClasses;
using static ThinBlueLie.Helper.ConfigHelper;
using ThinBlueLie.Helper.Algorithms;
using ThinBlueLie.Helper.Extensions;

namespace ThinBlueLie.Helper
{
    public class SearchesSubmit
    {
        public IDataAccess Data { get; }

        public SearchesSubmit(IDataAccess data)
        {
            Data = data;
        }

        private static Tuple<int, string> GetNameInfo(string Input)
        {
            if (string.IsNullOrWhiteSpace(Input))
            {
                return Tuple.Create(0, "null");
            }
            Input = StringExtensions.NormalizeWhiteSpace(Input);

            string[] subNames = Input.Split(' ');            
            var FirstName = subNames.FirstOrDefault();
            var LastName = subNames.Last();
            string NormalizedName;
            // If more than 1 name
            if (subNames.Length > 1)
                NormalizedName = FirstName + " " + LastName;
            else
                NormalizedName = FirstName;

            //TODO for names like: J. Alexander Kueng or make it into J. Kueng and Alexander Kueng and test both
            //Remove first name, remove last element, reverse back to normal
            var MiddleNames = subNames.Skip(1).Reverse().Skip(1).Reverse().ToArray();
            int MiddleCount;
            if (MiddleNames.Length > 1)
            {
                MiddleCount = MiddleNames.Length;
            }
            else
                MiddleCount = 0;
            return new Tuple<int, string>(MiddleCount, NormalizedName);
        }
        public async Task<List<SimilarPersonGeneral>> SearchOfficer(string Input)
        {
            var NameInfo = GetNameInfo(Input);            
            var NormalizedName = NameInfo.Item2;          
            int MiddleCount = NameInfo.Item1;     

            //Load all OfficerNames
            string sql = "SELECT IdOfficer, Name FROM officers";
            List<FirstLoadOfficer> Names = await Data.LoadData<FirstLoadOfficer, dynamic>(sql, new { });

            List<NameScore> nameScores = new List<NameScore>();
            foreach (var name in Names)
            {
                var nameInfo = GetNameInfo(name.Name);
                var normalizedName = nameInfo.Item2;
                int middleCount = nameInfo.Item1;                

                var score = JaroWinklerDistance.proximity(normalizedName, NormalizedName);               
                score += Math.Abs(middleCount - MiddleCount) / 40; //40 is just a random number change it later
                var listitem = new NameScore()
                {
                    Id = name.IdOfficer,                 
                    Score = score,
                };
                nameScores.Add(listitem);
            }
            //return list of names in order of lowest to highest score
            var SortedNames = nameScores.OrderByDescending(o => o.Score).ToList();
            var filteredIds = SortedNames.Where(s => s.Score > 0.7).Select(x => x.Id).ToArray(); //Only Ids with high enough score  
            var Ids = string.Join(",", filteredIds);
            List<SimilarOfficer> similarPeople;
            if (!string.IsNullOrWhiteSpace(Ids))
            {
                var sql2 = $"SELECT * FROM officers Where IdOfficer in ({Ids}) ORDER BY field(IdOfficer, {Ids})";
                similarPeople = await Data.LoadData<SimilarOfficer, dynamic>(sql2, new {});
                for (int i = 0; i < filteredIds.Length; i++)
                {
                    var sql3 = "SELECT distinct t.Date, t.City, t.State " +
                                   "FROM timelineinfo t " +
                                   "JOIN misconducts m ON t.IdTimelineinfo = m.IdTimelineinfo " +
                                   "JOIN officers o ON m.IdOfficer = o.IdOfficer " +
                                   "WHERE o.IdOfficer = @id;";
                    similarPeople[i].Events = await Data.LoadData<SimilarPerson.SimilarPersonEvents, dynamic>(sql3, new {Id = filteredIds[i] });
                }                          
            }
            else
            {
                similarPeople = new List<SimilarOfficer>();
            }
            List<SimilarPersonGeneral> SimilarPeople = new List<SimilarPersonGeneral>();
            foreach (var person in similarPeople)
            {
                //TODO simplify with AutoMapper
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
            var NameInfo = GetNameInfo(Input);
            var NormalizedName = NameInfo.Item2;
            int MiddleCount = NameInfo.Item1;

            //Load all SubjectNames
            string sql = "SELECT IdSubject, Name FROM subjects";
            List<FirstLoadSubject> Names = await Data.LoadData<FirstLoadSubject, dynamic>(sql, new { });

            List<NameScore> nameScores = new List<NameScore>();
            foreach (var name in Names)
            {
                var nameInfo = GetNameInfo(name.Name);
                var normalizedName = nameInfo.Item2;
                int middleCount = nameInfo.Item1;

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
            var filteredIds = SortedNames.Where(s => s.Score > 0.7).Select(x => x.Id).ToArray(); //Only Ids with high enough score  
            var Ids = string.Join(",", filteredIds);
            List<SimilarSubject> similarPeople;
            if (!string.IsNullOrWhiteSpace(Ids))
            {
                var sql2 = $"SELECT * FROM subjects Where IdSubject in ({Ids}) ORDER BY field(IdSubject, {Ids})";
                similarPeople = await Data.LoadData<SimilarSubject, dynamic>(sql2, new {});
                for (int i = 0; i < filteredIds.Length; i++)
                {
                    var sql3 = "SELECT distinct t.Date, t.City, t.State " +
                                   "FROM timelineinfo t " +
                                   "JOIN misconducts m ON t.IdTimelineinfo = m.IdTimelineinfo " +
                                   "JOIN subjects s ON m.IdSubject = s.IdSubject " +
                                   "WHERE s.IdSubject = @Id;";
                    similarPeople[i].Events = await Data.LoadData<SimilarPerson.SimilarPersonEvents, dynamic>(sql3, new {Id = filteredIds[i] });
                }
            }
            else
            {
                similarPeople = new List<SimilarSubject>();
            }
            List<SimilarPersonGeneral> SimilarPeople = new List<SimilarPersonGeneral>();
            foreach (var person in similarPeople)
            {
                //TODO simplify with AutoMapper
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
            if (TempDate == null)
            {
                return SimilarEvents;
            }
            var query1 = "SELECT * From timelineinfo t where t.date = @Date;";
            var SimilarTimelineinfos = await Data.LoadData<Timelineinfo, dynamic>(query1, new {Date = TempDate });
            foreach ((var Event, int i) in SimilarTimelineinfos.Select((Event, i) => (Event, i)))
            {
                var Id = Event.IdTimelineinfo;
                var officerQuery = @"Select distinct o.* from officers o
                                        JOIN misconducts m on o.IdOfficer = m.IdOfficer
                                        WHERE m.IdTimelineinfo = 1;";
                var Officers = await Data.LoadData<CommonPerson, dynamic>(officerQuery, new { id = Id });
                var subjectQuery = @"Select distinct s.* from subjects s
                                        JOIN misconducts m on s.IdSubject = m.IdSubject
                                        WHERE m.IdTimelineinfo = 1;";
                var Subjects = await Data.LoadData<CommonPerson, dynamic>(subjectQuery, new { id = Id });
                var MediaQuery = "Select *,(true) as Processed From media m Where(m.IdTimelineinfo = @id);";
                var Media = await Data.LoadData<ViewMedia, dynamic>(MediaQuery, new {id = Id });

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
            await ViewMedia.GetDataMany(SimilarEvents.Select(e => e.Media).ToList());

            return SimilarEvents;
        }
    }   
    
}

