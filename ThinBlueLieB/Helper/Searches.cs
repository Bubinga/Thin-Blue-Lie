using DataAccessLibrary.DataAccess;
using DataAccessLibrary.DataModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLieB.Models;
using static ThinBlueLieB.Helper.Algorithms;
using static ThinBlueLieB.Models.ViewSimilar;

namespace ThinBlueLieB.Helper
{
    public class Searches : ISearches
    {
        private readonly ConnectionStrings connectionStrings;
        public Searches(IOptions<ConnectionStrings> options)
        {
            connectionStrings = options.Value;
        }

        public async Task<List<SimilarPerson>> SearchOfficer(string Input)
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
            List<FirstLoadOfficer> Names = await data.LoadData<FirstLoadOfficer, dynamic>(sql, new { }, connectionStrings.DataDB);

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
            string sql2 = "SELECT * FROM officers Where IdOfficer in (" + Ids + ") ORDER BY field(IdOfficer, " + Ids + ")";
            List<SimilarPerson> similarPeople = await data.LoadData<SimilarPerson, dynamic>(sql2, new { }, connectionStrings.DataDB);
            return similarPeople;           
        }

        public async Task<List<ViewSimilar>> GetSimilar(string? TempDate)
        {
            //load events where date or officer/subject name is shared and load it into SimilarEvents.            
            List<ViewSimilar> SimilarEvents = new List<ViewSimilar>();
            DataAccess data = new DataAccess();
            var query1 = "SELECT t.Date, t.IdTimelineinfo, t.Context, t.State, t.City, t.Verified, t.SubmittedBy From timelineinfo t where t.date = " + TempDate + ";";
            var SimilarTimelineinfos = await data.LoadData<Timelineinfo, dynamic>(query1, new { }, connectionStrings.DataDB);
            foreach ((var Event, Int32 i) in SimilarTimelineinfos.Select((Event, i) => (Event, i)))
            {
                var id = Event.IdTimelineinfo;
                var officerQuery = "SELECT o.Name, o.Race, o.Sex, o.Age FROM timelineinfo JOIN timelineinfo_officer ON timelineinfo.IdTimelineinfo = timelineinfo_officer.IdTimelineinfo JOIN officers o ON timelineinfo_officer.IdOfficer = o.IdOfficer WHERE timelineinfo.IdTimelineinfo = " + id + ";";
                var Officers = await data.LoadData<ViewSimilarPerson, dynamic>(officerQuery, new { }, connectionStrings.DataDB);
                var subjectQuery = "SELECT o.Name, o.Race, o.Sex, o.Age FROM timelineinfo JOIN timelineinfo_subject ON timelineinfo.IdTimelineinfo = timelineinfo_subject.IdTimelineinfo JOIN subjects o ON timelineinfo_subject.IdSubject = o.IdSubject WHERE timelineinfo.IdTimelineinfo = " + id + ";";
                var Subjects = await data.LoadData<ViewSimilarPerson, dynamic>(officerQuery, new { }, connectionStrings.DataDB);
                var MediaQuery = "Select * From media m Where(m.IdTimelineinfo = " + id + ";";
                var Media = await data.LoadData<Media, dynamic>(officerQuery, new { }, connectionStrings.DataDB);

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

        public async Task<List<List<Timelineinfo>>> GetTimeline(string? current, int? dateChange, string? date)
        {
            //string[] weekDays = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            if (current != null && dateChange != null){
                date = Convert.ToDateTime(current).AddDays((double)dateChange).ToString("yyyy-MM-dd");
            }
            else if (date == null | string.IsNullOrWhiteSpace(Extensions.GetQueryParm("d"))){
                date = DateTime.Today.ToString("yyyy-MM-dd");
            }
            else{
                date = Extensions.GetQueryParm("d");
            }
            DateTime dateT = Convert.ToDateTime(date); //convert date from string to DateTime
            DateTime[] dates = new DateTime[7];
            var weekStart = dateT.AddDays(-(int)dateT.DayOfWeek); //week start date
            if (dateChange == null){
                dates[0] = dateT.AddDays(-(int)dateT.DayOfWeek);
            }
            else {
                dates[0] = dateT.AddDays(-(int)dateT.DayOfWeek);
            }
            for (int i = 1; i < 7; i++) {
                dates[i] = weekStart.AddDays(i);
            }
            var dateData = new List<List<Timelineinfo>>(new List<Timelineinfo>[7]);
            for (int i = 0; i < 7; i++)
            {
                //get data
                DataAccess data = new DataAccess();
                var query = "SELECT t.TimelineinfoId From timelineinfo t where t.date = " + dates[i].ToString("yyyy-MM-dd") + ";";
                dateData[i] = await data.LoadData<Timelineinfo, dynamic>(query, new { }, connectionStrings.DataDB);
            }
            return dateData;
        }
    }
    
    public class FirstLoadOfficer
    {
        public int IdOfficer { get; set; }
        public string Name { get; set; }
    }
    public class NameScore
    {
        public int Id { get; set; }
        //public string Name { get; set; }
        public double Score { get; set; }
        //public int MiddleNameCount { get; set; }
    }
    public class SimilarPerson
    {
        public class SimilarPersonEvents
        {
            public DateTime Date { get; set; }
            public byte State { get; set; }
            public string City { get; set; }
        }
        public string Name { get; set; }
        public byte Race { get; set; }
        public byte Sex { get; set; }
        public List<SimilarPersonEvents> Events { get; set; }

    }
}

