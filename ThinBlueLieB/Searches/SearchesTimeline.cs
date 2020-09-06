using DataAccessLibrary.DataAccess;
using DataAccessLibrary.DataModels;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThinBlueLieB.Helper;

namespace ThinBlueLieB.Searches
{
    public class SearchesTimeline
    {
        [Inject]
        private ConnectionStringService connectionStrings { get; }
        //private readonly ConnectionStringService connectionStrings;
        //public SearchesTimeline(IOptions<ConnectionStringService> options)
        //{
        //    connectionStrings = options.Value;
        //}

        public async Task<List<List<Timelineinfo>>> GetTimeline(string? current, int? dateChange, string? date)
        {
            //string[] weekDays = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            if (current != null && dateChange != null)
            {
                date = Convert.ToDateTime(current).AddDays((double)dateChange).ToString("yyyy-MM-dd");
            }
            else if (date == null | string.IsNullOrWhiteSpace(Extensions.GetQueryParm("d")))
            {
                date = DateTime.Today.ToString("yyyy-MM-dd");
            }
            else
            {
                date = Extensions.GetQueryParm("d");
            }
            DateTime dateT = Convert.ToDateTime(date); //convert date from string to DateTime
            DateTime[] dates = new DateTime[7];
            var weekStart = dateT.AddDays(-(int)dateT.DayOfWeek); //week start date
            if (dateChange == null)
            {
                dates[0] = dateT.AddDays(-(int)dateT.DayOfWeek);
            }
            else
            {
                dates[0] = dateT.AddDays(-(int)dateT.DayOfWeek);
            }
            for (int i = 1; i < 7; i++)
            {
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
}
