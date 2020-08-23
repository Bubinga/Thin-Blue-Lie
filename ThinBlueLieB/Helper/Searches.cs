using DataAccessLibrary.DataAccess;
using Microsoft.Extensions.Configuration;
using Syncfusion.Blazor.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ThinBlueLieB.Helper.Algorithms;

namespace ThinBlueLieB.Helper
{
    public class Searches
    {
        private readonly IConfiguration _config;

        public Searches(IConfiguration configuration)
        {
            _config = configuration;
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
            public int MiddleNameCount { get; set; }
        }        
        public async Task<List<NameScore>> SearchOfficer(string Input)
        {            
            //Get only First and Last Name
            var FirstName = Input.Split(' ').FirstOrDefault();
            var LastName = Input.Split(' ').Last();
            var InputName = FirstName + " " + LastName;
            //don't remove first name if it's something like J. Alexander Kueng
            var MiddleCount = Input.Replace(LastName, "").Replace(FirstName, "").Split(' ').Count();

            //Load all OfficcerNames
            DataAccess data = new DataAccess();
            string sql = "SELECT IdOfficer, Name FROM officers";
            List<FirstLoadOfficer> Names = await data.LoadData<FirstLoadOfficer, dynamic>(sql, new { }, _config.GetConnectionString("DataDB"));

            List<NameScore> nameScores = new List<NameScore>();
            foreach (var name in Names)
            {
                var first = name.Name.Split(' ').FirstOrDefault();
                var last = name.Name.Split(' ').Last();
                var NormalizedName = first + " " + last;
                var middlecount = name.Name.Replace(first, "").Replace(last, "").Split(' ').Count();              

                var score = JaroWinklerDistance.distance(name.Name, InputName);
                score = score + Math.Abs(middlecount - MiddleCount)/40; //40 is just a random number change it later
                var listitem = new NameScore()
                {
                    Id = name.IdOfficer,
                    Name = NormalizedName,
                    Score = score,
                };
                nameScores.Add(listitem);
            }
            var SortedNames = nameScores.OrderBy(o=>o.Score).ToList();
            return SortedNames;
            

            //return list of names in order of lowest to highest score
        }
    }
}
    