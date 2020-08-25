using DataAccessLibrary.DataAccess;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ThinBlueLieB.Helper.Algorithms;

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
                score = score + Math.Abs(middleCount - MiddleCount) / 40; //40 is just a random number change it later
                var listitem = new NameScore()
                {
                    Id = name.IdOfficer,
                    Name = normalizedName,
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
    }
    public class FirstLoadOfficer
    {
        public int IdOfficer { get; set; }
        public string Name { get; set; }
    }
    public class NameScore
    {
        public int Id { get; set; }
        public string Name { get; set; }
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

