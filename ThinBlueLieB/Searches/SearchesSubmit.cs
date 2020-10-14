using DataAccessLibrary.DataAccess;
using DataAccessLibrary.DataModels;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLieB.Models;
using static ThinBlueLieB.Models.ViewSimilar;
using static ThinBlueLieB.Searches.SearchClasses;
using static ThinBlueLieB.Helper.ConnectionStringHelper;
using ThinBlueLieB.Helper.Algorithms;

namespace ThinBlueLieB.Helper
{
    public class SearchesSubmit
    {      

        private Tuple<int, string> GetNameInfo(string Input)
        {
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
            return new Tuple<int, string>(MiddleCount, NormalizedName);
        }
        public async Task<List<SimilarPersonGeneral>> SearchOfficer(string Input)
        {
            var NameInfo = GetNameInfo(Input);            
            var NormalizedName = NameInfo.Item2;          
            int MiddleCount = NameInfo.Item1;     

            //Load all OfficerNames
            DataAccess data = new DataAccess();
            string sql = "SELECT IdOfficer, Name FROM officers";
            List<FirstLoadOfficer> Names = await data.LoadData<FirstLoadOfficer, dynamic>(sql, new { }, GetConnectionString());

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
            var Ids = String.Join(",", SortedNames.Where(s => s.Score > 0.7).Select(x => x.Id));            
            List<SimilarOfficer> similarPeople;
            if (!string.IsNullOrWhiteSpace(Ids))
            {
                var sql2 = "SELECT o.Name, o.Race, o.Sex, IdOfficer FROM officers o Where IdOfficer in (@IdList) ORDER BY field(IdOfficer, @IdList)";
                similarPeople = await data.LoadData<SimilarOfficer, dynamic>(sql2, new {IdList = Ids }, GetConnectionString());
                for (int i = 0; i < Ids.Length; i++)
                {
                    var sql3 = "SELECT t.Date, t.City, t.State " +
                                "FROM timelineinfo t " +
                                "JOIN timelineinfo_officer ON t.IdTimelineinfo = timelineinfo_officer.IdTimelineinfo " +
                                "JOIN officers o ON timelineinfo_officer.IdOfficer = o.IdOfficer " +
                                "WHERE o.IdOfficer = @Id;";
                    similarPeople[i].Events = await data.LoadData<SimilarPerson.SimilarPersonEvents, dynamic>(sql3, new {Id = Ids[i] }, GetConnectionString());
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
            DataAccess data = new DataAccess();
            string sql = "SELECT IdSubject, Name FROM subjects";
            List<FirstLoadSubject> Names = await data.LoadData<FirstLoadSubject, dynamic>(sql, new { }, GetConnectionString());

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
            var Ids = String.Join(",", SortedNames.Where(s => s.Score > 0.7).Select(x => x.Id));   //deliminated ids of possible names        
            List<SimilarSubject> similarPeople;
            if (!string.IsNullOrWhiteSpace(Ids))
            {
                var sql2 = "SELECT * FROM subjects Where IdSubject in (@IdList) ORDER BY field(IdSubject, @IdList)";
                similarPeople = await data.LoadData<SimilarSubject, dynamic>(sql2, new {IdList = Ids }, GetConnectionString());
                for (int i = 0; i < Ids.Length; i++)
                {
                    var sql3 = "SELECT t.Date, t.City, t.State " +
                                "FROM timelineinfo t " +
                                "JOIN timelineinfo_subject ON t.IdTimelineinfo = timelineinfo_subject.IdTimelineinfo " +
                                "JOIN subjects s ON timelineinfo_subject.IdSubject = s.IdSubject " +
                                "WHERE s.IdSubject = @Id;";
                    similarPeople[i].Events = await data.LoadData<SimilarSubject.SimilarPersonEvents, dynamic>(sql3, new {Id = Ids[i] }, GetConnectionString());
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
            DataAccess data = new DataAccess();
            var query1 = "SELECT t.Date, t.IdTimelineinfo, t.Context, t.State, t.City, t.Verified From timelineinfo t where t.date = @Date;";
            var SimilarTimelineinfos = await data.LoadData<Timelineinfo, dynamic>(query1, new {Date = TempDate }, GetConnectionString());
            foreach ((var Event, Int32 i) in SimilarTimelineinfos.Select((Event, i) => (Event, i)))
            {
                var Id = Event.IdTimelineinfo;
                var officerQuery = "SELECT o.Name, o.Race, o.Sex, tio.Age FROM timelineinfo t JOIN timelineinfo_officer tio ON t.IdTimelineinfo = tio.IdTimelineinfo JOIN officers o ON tio.IdOfficer = o.IdOfficer WHERE t.IdTimelineinfo = @id;";
                var Officers = await data.LoadData<ViewSimilarPerson, dynamic>(officerQuery, new { id = Id }, GetConnectionString());
                var subjectQuery = "SELECT s.Name, s.Race, s.Sex, ts.Age FROM timelineinfo t JOIN timelineinfo_subject ts ON t.IdTimelineinfo = ts.IdTimelineinfo JOIN subjects s ON ts.IdSubject = s.IdSubject WHERE t.IdTimelineinfo = @id;";
                var Subjects = await data.LoadData<ViewSimilarPerson, dynamic>(subjectQuery, new { id = Id }, GetConnectionString());
                var MediaQuery = "Select * From media m Where(m.IdTimelineinfo = @id);";
                var Media = await data.LoadData<Media, dynamic>(MediaQuery, new {id = Id }, GetConnectionString());

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

