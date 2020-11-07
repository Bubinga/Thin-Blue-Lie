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
        public UserManager<ApplicationUser> userManager { get; set; }
        [Inject]
        public RoleManager<IdentityRole> roleManager { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> _authState { get; set; }
        private AuthenticationState userState;
        ApplicationUser User;
        string UserId;
        int PendingEditsCount;
        IEnumerable<FirstLoadEditHistory> Ids;
        int ActiveIdIndex;
        List<ViewEvent> Edits = new List<ViewEvent>();
        [Inject]
        SearchesEditReview review { get; set; }
        protected override async Task OnInitializedAsync()
        {
            //TODO only trigger this if use is logged in
            userState = await _authState;
            User = await userManager.GetUserAsync(userState.User);
            UserId = User.Id;
            await GetEdits();
        }
        async Task GetEdits()
        {
            bool canReviewAll = await userManager.IsInRoleAsync(User, "GreatUser");
            if (canReviewAll == false)
                Ids = await review.GetPendingEdits(User);               
            else 
                Ids = await review.GetPendingEdits(User, true);      
            
            //get number of all the unconfirmed edits that user can access
            PendingEditsCount = Ids.Count();
            var firstId = Ids.FirstOrDefault().IdTimelineinfo;
            if (firstId != 0) //firstId will never be zero other than if ids is empty
            {
                Edits.Add(await review.GetEditFromId(firstId));
                ActiveIdIndex = 0;
            }
        }
        async Task GetNextEdit()
        {
            Edits.Add(await review.GetEditFromId(Ids.Select(x => x.IdTimelineinfo).ToArray()[ActiveIdIndex]));
            ActiveIdIndex++;
        }
    }
}
