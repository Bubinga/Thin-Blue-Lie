using DataAccessLibrary.UserModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLieB.Models;
using ThinBlueLieB.Models.View_Models;

namespace ThinBlueLieB.Bases
{
    public class EditReviewBase : ComponentBase
    {
        public EditReviewModel versions = new EditReviewModel();
        //ensure editcount is same for all across, so get editcount from edits and use the same number against the others
        //highest edit count where confirmed = 1 is the active one
        //return two rows for all queries, the first being active, second being inactive
        //load on arrow click, get a list of all edits which have either
        //  the original post having a submittedby matching the user
        //      edits where editcount = 0
        //  the original post being a community post
        [Inject]
        public UserManager<ApplicationUser> userManager { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> _authState { get; set; }
        private AuthenticationState userState;
        protected override async Task OnInitializedAsync()
        {
            userState = await _authState;
            var userId = userManager.GetUserId(userState.User);
            
        }
    }
}
