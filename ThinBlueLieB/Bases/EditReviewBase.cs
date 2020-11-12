using AutoMapper;
using DataAccessLibrary.UserModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using MySql.Data.MySqlClient;
using Syncfusion.Blazor.PdfViewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLieB.Helper;
using ThinBlueLieB.Models;
using ThinBlueLieB.Models.View_Models;
using ThinBlueLieB.Searches;
using ThinBlueLieB.ViewModels;
using static ThinBlueLieB.Searches.SearchClasses;

namespace ThinBlueLieB.Bases
{
    public class EditReviewBase : ComponentBase
    {
        public EditReviewModel versions = new EditReviewModel();
        //ensure editcount is same for all across, so get editcount from edits and use the same number against the others
        //highest edit count where confirmed = 1 is the active one
        //return two rows for all queries, the first being active, second being inactive
        //load on arrow click, get a list of all edits which have either
        //      the original post having a submittedby matching the user
        //      edits where editcount = 0
        //      the original post being a community post
        //  store status of post in community column
        [Inject]
        public UserManager<ApplicationUser> UserManager { get; set; }
        [Inject]
        public RoleManager<IdentityRole> RoleManager { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> AuthState { get; set; }
        private AuthenticationState userState;
        ApplicationUser User;
        public int PendingEditsCount;
        public IEnumerable<FirstLoadEditHistory> Ids;
        public int ActiveIdIndex;
        public bool Loading = true;
        public List<EditReviewModel> Edits = new List<EditReviewModel>();
        [Inject]
        SearchesEditReview Review { get; set; }
        protected override async Task OnInitializedAsync()
        {
            //TODO only trigger this if use is logged in
            userState = await AuthState;
            User = await UserManager.GetUserAsync(userState.User);            
            Loading = true;
            await GetEdits();
        }
        async Task GetEdits()
        {
            bool canReviewAll = await UserManager.IsInRoleAsync(User, "GreatUser");
            if (canReviewAll == false)
                Ids = await Review.GetPendingEdits(User);               
            else 
                Ids = await Review.GetPendingEdits(User, true);

            //get number of all the unconfirmed edits that user can access
            var firstId = Ids.FirstOrDefault().IdTimelineinfo;
            if (firstId != 0) //firstId will never be zero other than if ids is empty
            {
                Edits.Add(await Review.GetEditFromId(Ids.FirstOrDefault()));
                ActiveIdIndex = 0;
            }
            PendingEditsCount = Ids.Count();
            Loading = false;
        }
        public async Task GetNextEdit()
        {
            Loading = true;
            if (Edits.Count < ActiveIdIndex)
            {
                Edits.Add(await Review.GetEditFromId(Ids.ToArray()[ActiveIdIndex]));
            }
            ActiveIdIndex++;
            Loading = false;
            this.StateHasChanged();
        }
        public void GetPreviousEdit()
        {
            ActiveIdIndex--;
            this.StateHasChanged();
        }
    }
}
