using DataAccessLibrary.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using static ThinBlueLie.Helper.ConfigHelper;
using System.Threading.Tasks;
using DataAccessLibrary.DataModels;

namespace ThinBlueLie.Searches
{
    public class SearchesFlagReview
    {
        private readonly IDataAccess data;

        public SearchesFlagReview(IDataAccess data)
        {
            this.data = data;
        }
        public async Task<int[]> GetPendingFlags()
        {
            string pendingFlagsSql = "Select IdFlags from Flags where Status = 0;";
            List<int> pendingId = await data.LoadData<int, dynamic>(pendingFlagsSql, new { });
            return pendingId?.ToArray();
        }

        public async Task<Flags> GetFlag(int IdFlag)
        {
            //Get the details of the flag and it's event's title
            string getFlagSql = "Select f.*,t.Title as EventTitle, t.Date From Flags f Join timelineinfo t On t.IdTimelineinfo = f.IdTimelineinfo Where f.IdFlags = @IdFlag;";
            Flags Flag = await data.LoadDataSingle<Flags, dynamic>(getFlagSql, new { IdFlag = IdFlag });
            return Flag;
        }
    }
}
