using DataAccessLibrary.DataAccess;
using DataAccessLibrary.DataModels;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThinBlueLie.Helper.Extensions;
using ThinBlueLie.Models.ViewModels;
using static ThinBlueLie.Helper.ConfigHelper;
using static ThinBlueLie.Models.ViewModels.TimelineinfoFull;

namespace ThinBlueLie.Searches
{
    public class SearchesTimeline
    {      
        public async Task<Tuple<List<List<TimelineinfoFull>>, DateTime[]>> GetTimeline(DateTime date)
        {
            //get dates[] 
            //get information for each date
          
            DateTime[] dates = new DateTime[7];           
            dates[0] = date.AddDays(-(int)date.DayOfWeek); //set first day of week           
            for (int i = 1; i < 7; i++)
            {
                dates[i] = dates[0].AddDays(i);
            }
            var dateData = new List<List<TimelineinfoFull>>(new List<TimelineinfoFull>[7]);
            for (int i = 0; i < 7; i++)
            {
                //get data
                DataAccess data = new DataAccess();
                var query = "SELECT IdTimelineinfo, Date, Title From timelineinfo where Date = @date;";                
                var result = await data.LoadData<Timelineinfo, dynamic>(query, new {date = dates[i].ToString("yyyy-MM-dd") }, GetConnectionString());
                List<TimelineinfoFull> results = new List<TimelineinfoFull>();
                foreach (var Event in result)
                {
                    var officerquery = "SELECT Misconduct,Weapon FROM timelineinfo_officer Where IdTimelineinfo = @id;";
                    var officerresult = await data.LoadData<TimelineinfoOfficerShort, dynamic>(officerquery, new {id = Event.IdTimelineinfo}, GetConnectionString());
                    results.Add(new TimelineinfoFull {Timelineinfo = Event, OfficerInfo = officerresult });
                }
                dateData[i] = results;
            }
            return Tuple.Create(dateData, dates);
        }
    }
}
