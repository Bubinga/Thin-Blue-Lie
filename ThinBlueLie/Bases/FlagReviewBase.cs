using DataAccessLibrary.DataAccess;
using DataAccessLibrary.DataModels;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLie.Searches;
using ThinBlueLie.Helper.Extensions;
using DataAccessLibrary.UserModels;
using Microsoft.AspNetCore.Identity;
using static DataAccessLibrary.Enums.ReputationEnum;

namespace ThinBlueLie.Bases
{
    public class FlagReviewBase : ComponentBase
    {
        [Inject]
        public UserManager<ApplicationUser> userManager { get; set; }
        [Inject]
        IDataAccess Data { get; set; }
        [Inject]
        SearchesFlagReview Review { get; set; }
        public List<Flags> Flags = new();
        public int[] Ids;
        public int PendingFlagsCount;
        public int ActiveIdIndex;
        public bool Loading;
        protected override async Task OnInitializedAsync()
        {
            Loading = true;
            Ids = await Review.GetPendingFlags();
            if (Ids == null || Ids.Any() is false)
            {
                PendingFlagsCount = 0;
                Loading = false;
            }
            else
            {                
                var firstId = Ids.First();                
                var flag = await Review.GetFlag(firstId);
                Flags.Add(flag);
                ActiveIdIndex = 0;
            }
            PendingFlagsCount = Ids.Length;
            Loading = false;
        }
        public async Task GetNextFlag()
        {
            if (PendingFlagsCount is 0)
                return;
            Loading = true;
            //if it hasnt already been loaded into Flags, load it
            if (Flags.Count - 1 <= ActiveIdIndex)
            {
                var flag = await Review.GetFlag(Ids[ActiveIdIndex == PendingFlagsCount - 1 ? 0 : ActiveIdIndex + 1]);
                Flags.Add(flag);
            }
            // cycle around to the start
            if (ActiveIdIndex == PendingFlagsCount - 1)
                ActiveIdIndex = 0;
            else
                ActiveIdIndex++;

            Loading = false;
            StateHasChanged();
        }
        public void GetPreviousFlag()
        {
            if (PendingFlagsCount is 0)
                return;
            if (ActiveIdIndex != 0)
                ActiveIdIndex--;
            StateHasChanged();
        }
        //Status of 0 is unreviewed, 1 is resolved, 2 is to be deleted
        public async Task ResolveFlag()
        { 
            await UpdateFlagStatus(1);
            await userManager.ChangeReputation(ReputationChangeEnum.HelpFulFlag, (int)Flags[ActiveIdIndex].UserId);
        }
        public async Task DeleteFlag()
        { await UpdateFlagStatus(2); }
        public async Task UpdateFlagStatus(int Status)
        {
            string updateFlagSql = "Update flags set Status = @Status where IdFlags = @IdFlags";
            await Data.SaveData(updateFlagSql, new {Status, IdFlags = Ids[ActiveIdIndex] });
            Flags.RemoveAt(ActiveIdIndex);
            Ids.RemoveAt(ActiveIdIndex);
            if (ActiveIdIndex != 0)
                ActiveIdIndex--;
            PendingFlagsCount--;
        }
    }
}
