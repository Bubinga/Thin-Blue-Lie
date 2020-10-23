using DataAccessLibrary.DataAccess;
using DataAccessLibrary.DataModels;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThinBlueLieB.Helper.Extensions;
using static ThinBlueLieB.Helper.ConfigHelper;

namespace ThinBlueLieB.Searches
{
    public class SearchesTimeline
    {      
        public async Task<Tuple<List<List<Timelineinfo>>, DateTime[]>> GetTimeline(DateTime date)
        {
            //get dates[] 
            //get information for each date
          
            DateTime[] dates = new DateTime[7];           
            dates[0] = date.AddDays(-(int)date.DayOfWeek); //set first day of week           
            for (int i = 1; i < 7; i++)
            {
                dates[i] = dates[0].AddDays(i);
            }
            var dateData = new List<List<Timelineinfo>>(new List<Timelineinfo>[7]);
            for (int i = 0; i < 7; i++)
            {
                //get data
                DataAccess data = new DataAccess();
                var query = "SELECT t.IdTimelineinfo From timelineinfo t where t.date = @date;";
                dateData[i] = await data.LoadData<Timelineinfo, dynamic>(query, new {date = dates[i].ToString("yyyy-MM-dd") }, GetConnectionString());
            }
            return Tuple.Create(dateData, dates);
        }
    }
}
