using Dapper;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.DataModels;
using DataAccessLibrary.DataModels.EditModels;
using DataAccessLibrary.UserModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLie.Models.View_Models;
using ThinBlueLie.Searches;
using static DataAccessLibrary.Enums.EditEnums;
using static ThinBlueLie.Helper.ConfigHelper;
using static ThinBlueLie.Searches.SearchClasses;

namespace ThinBlueLie.Bases
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
        public RoleManager<ApplicationRole> RoleManager { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> AuthState { get; set; }
        private AuthenticationState userState;
        ApplicationUser User;
        public int PendingEditsCount;
        public IEnumerable<FirstLoadEditHistory> Ids;
        public int ActiveIdIndex;
        public bool Loading = true;
        public List<EditHistory> EditChanges = new List<EditHistory>(); 
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
            Ids = await Review.GetPendingEdits(User);               

            //get number of all the unconfirmed edits that user can access
            var firstId = Ids.FirstOrDefault().IdTimelineinfo;
            if (firstId != 0) //firstId will never be zero other than if ids is empty
            {
                var change = await Review.GetEditFromId(Ids.FirstOrDefault());
                Edits.Add(change.Item1);
                EditChanges.Add(change.Item2);
                ActiveIdIndex = 0;
            }
            PendingEditsCount = Ids.Count();
            Loading = false;
        }
        public async Task GetNextEdit()
        {
            Loading = true;
            //if it hasnt already been loaded
            if (Edits.Count - 1 <= ActiveIdIndex)
            {
                var change = await Review.GetEditFromId(Ids.ToArray()[ActiveIdIndex == PendingEditsCount - 1 ? 0 : ActiveIdIndex + 1]);
                Edits.Add(change.Item1);
                EditChanges.Add(change.Item2);
            }
            if (ActiveIdIndex == PendingEditsCount - 1)
                ActiveIdIndex = 0;
            else
                ActiveIdIndex++;
            Loading = false;
            StateHasChanged();
        }
        public void GetPreviousEdit()
        {
            if (ActiveIdIndex == 0)
                ActiveIdIndex = PendingEditsCount - 1;
            else
                ActiveIdIndex--;
            StateHasChanged();
        }

        DataAccess data = new DataAccess();
        //TODO make only able to vote once.
        public async Task AcceptEdit()
        {
            //TODO add check if the edit was already accepted, people could be sitting with a tab open for days
            //Add super accept option for trusted users that has a vote value of 2
            var Vote = Ids.ToArray()[ActiveIdIndex].Vote;
            //if the vote was already accepted
            if (Vote < 1 || Vote == null)
            {
                string AcceptSql = @"INSERT INTO edit_votes (IdEditHistory,UserId,Vote) 
                                            VALUES (@IdEditHistory, @UserId, 1) AS new
                                               ON DUPLICATE KEY UPDATE
                                                   Vote = new.Vote;
                                    SELECT e.Vote FROM edit_votes e where e.IdEditHistory = @IdEditHistory;";
                IEnumerable<int> rows;
                using (IDbConnection connection = new MySqlConnection(GetConnectionString()))
                {
                    rows = await connection.QueryAsync<int>(AcceptSql, new { IdEditHistory = Ids.ToArray()[ActiveIdIndex].IdEditHistory, UserId = User.Id });
                }
                Ids.ToArray()[ActiveIdIndex].Vote = 1;
                if (rows.Sum() <= 2)
                    await MergeEdit();
                //StateHasChanged();
            }
           
        }
        public async Task RejectEdit()
        {
            var Vote = Ids.ToArray()[ActiveIdIndex].Vote;
            if (Vote > -1 || Vote == null)
            {
                string AcceptSql = @"INSERT INTO edit_votes (`IdEditHistory`, `UserId`, `Vote`) 
                                            VALUES(@IdEditHistory, @UserId, -1) AS new
                                              ON DUPLICATE KEY UPDATE
                                                  Vote = new.Vote; ";
                await data.SaveData(AcceptSql, new { Ids.ToArray()[ActiveIdIndex].IdEditHistory, UserId = User.Id }, GetConnectionString());
                Ids.ToArray()[ActiveIdIndex].Vote = -1;
            }
        }

        public async Task MergeEdit()
        {
            var change = EditChanges.ToArray()[ActiveIdIndex];
            var newInfo = Edits.ToArray()[ActiveIdIndex].New;

            //TODO change system so that I don't have to query this information twice
            using (IDbConnection connection = new MySqlConnection(GetConnectionString()))
            {
                if (change.Edits == 1)
                {
                    string EditChangedQuery = "INSERT INTO timelineinfo(`IdTimelineinfo`, `Title`, `Date`, `State`, `City`, `Context`, `Locked`, `Owner`)"+
                                                   $"VALUES(@IdTimelineinfo, @Title, @Date, @State, @City, @context, @Locked, '{Ids.ToArray()[ActiveIdIndex].SubmittedBy}')" +
                                                "ON DUPLICATE KEY UPDATE " +
                                                   "`Title` = @Title, `Date` = @Date, `State` = @State, " +
                                                   "`City` = @City, `Context` = @Context, " +
                                                   $"`Locked` = @Locked, `Owner` = '{Ids.ToArray()[ActiveIdIndex].SubmittedBy}';";
                    await connection.ExecuteAsync(EditChangedQuery, newInfo.Data);
                }
                if (change.EditMedia == 1)
                {
                    string MediaChangedQuery = @"Select * From editmedia m Where m.IdEditHistory = @id;";
                    var changesToMedia = await data.LoadData<EditMedia, dynamic>(MediaChangedQuery, new { id = change.IdEditHistory }, GetConnectionString());                   
                    //TODO find way to do all at once. 
                    foreach (var media in changesToMedia)
                    {
                        var action = (EditActions)media.Action;
                        string mediaQuery = string.Empty;
                        if (action == EditActions.Addition)
                            mediaQuery = "Insert into media (`IdTimelineinfo`, `MediaType`, `SourcePath`, `Gore`, " +
                                             "`SourceFrom`, `Blurb`, `Credit`, `SubmittedBy`, `Rank`) " +
                                             $"VALUES (@IdTimelineinfo, @MediaType, @SourcePath, @Gore, " +
                                             $"@SourceFrom, @Blurb, @Credit, @SubmittedBy, @Rank); ";

                        else if (action == EditActions.Update)
                            mediaQuery = @$"UPDATE media SET 
                                            `MediaType` = @MediaType, `Gore` = @Gore, `SourceFrom` = @SourceFrom, 
                                            `Blurb` = @Blurb, `Credit` = @Credit, `SubmittedBy` = @SubmittedBy,
                                            `Rank` = @Rank WHERE (`IdMedia` = @IdMedia);";

                        else if (action == EditActions.Deletion)
                            mediaQuery = $"DELETE FROM media WHERE (`IdMedia` = @IdMedia);";

                        await connection.ExecuteAsync(mediaQuery, media);
                    }
                }
                if (change.Officers == 1)
                {
                    string GetOfficerChanges = "Select * from edits_officer o where o.IdEditHistory = @id";
                    List<EditOfficer> changes = (List<EditOfficer>)await connection.QueryAsync<EditOfficer>(GetOfficerChanges, new {id = change.IdEditHistory});
                    //Small enough file and not editted enough for optimization
                    var firstAction = (EditActions)changes.FirstOrDefault().Action;
                    if (changes.Count > 1 || firstAction == EditActions.Addition)
                    {
                        string AddOfficer = "INSERT INTO officers (`IdOfficer`, `Name`, `Race`, `Sex`, `Image`, `Local`) " +
                                              "VALUES(@IdOfficer, @Name, @Race, @Sex, @Image, @Local);" ;
                        await connection.ExecuteAsync(AddOfficer, changes);
                    }
                    //can only have one
                    else
                    {
                        if (firstAction == EditActions.Update)
                        {
                            string UpdateOfficer = "UPDATE officers SET `Name` = @Name, `Race` = @Race, `Sex` = @Sex, " +
                                                      "`Image` = @Image, `Local` = @Local WHERE (`IdOfficer` = @IdOfficer);";
                            await connection.ExecuteAsync(UpdateOfficer, changes.FirstOrDefault());
                        }                       
                        if (firstAction == EditActions.Deletion)
                        {                         
                            string DeleteOfficer = "DELETE FROM officers WHERE (`IdOfficer` = @IdOfficer);";
                            await connection.ExecuteAsync(DeleteOfficer, changes.FirstOrDefault());
                        }
                    }  
                }
                if (change.Subjects == 1)
                {
                    string GetSubjectChanges = "Select * from edits_subject o where o.IdEditHistory = @id";
                    List<EditSubject> changes = (List<EditSubject>)await connection.QueryAsync<EditSubject>(GetSubjectChanges, new { id = change.IdEditHistory });
                    //Small enough file and not editted enough for optimization
                    var firstAction = (EditActions)changes.FirstOrDefault().Action;
                    if (changes.Count > 1 || firstAction == EditActions.Addition)
                    {
                        string AddSubject = @"INSERT INTO subjects (`IdSubject`, `Name`, `Race`, `Sex`, `Image`, `Local`) 
                                              VALUES(@IdSubject, @Name, @Race, @Sex, @Image, @Local);";
                        await connection.ExecuteAsync(AddSubject, changes);
                    }
                    //can only have one
                    else
                    {
                        if (firstAction == EditActions.Update)
                        {
                            string UpdateSubject = @"UPDATE subjects SET `Name` = @Name, `Race` = @Race, `Sex` = @Sex, 
                                                    `Image` = @Image, `Local` = @Local WHERE (`IdSubject` = @IdSubject);";
                            await connection.ExecuteAsync(UpdateSubject, changes.FirstOrDefault());
                        }
                        if (firstAction == EditActions.Deletion)
                        {
                            string DeleteSubject = "DELETE FROM subjects WHERE (`IdSubject` = @IdSubject);";
                            await connection.ExecuteAsync(DeleteSubject, changes.FirstOrDefault());
                        }
                    }
                }
                if (change.Timelineinfo_Officer == 1)
                {
                    string getTOChanges = "Select * from edits_timelineinfo_officer e Where e.IdEditHistory = @id";
                    var changes = await connection.QueryAsync<EditTimelineinfoOfficer>(getTOChanges, new { id = change.IdEditHistory});
                    string deleteEverything = "Delete from timelineinfo_officer t_o where t_o.IdTimelineinfo = @IdTimelineinfo;";
                    await connection.ExecuteAsync(deleteEverything, changes);
                    string applyChanges = @"INSERT INTO timelineinfo_officer (IdTimelineinfo, IdOfficer, Age, Misconduct, Weapon) 
                                            VALUES (@IdTimelineinfo, @IdOfficer, @Age, @Misconduct, @Weapon);";
                    await connection.ExecuteAsync(applyChanges, changes);
                }
                if (change.Timelineinfo_Subject == 1)
                {
                    string getTOChanges = "Select * from edits_timelineinfo_subject e Where e.IdEditHistory = @id";
                    var changes = await connection.QueryAsync<EditTimelineinfoSubject>(getTOChanges, new { id = change.IdEditHistory });
                    string deleteEverything = "Delete from timelineinfo_subject ts where ts.IdTimelineinfo = @IdTimelineinfo;";
                    await connection.ExecuteAsync(deleteEverything, changes);
                    string applyChanges = @"INSERT INTO timelineinfo_subject (IdTimelineinfo, IdSubject, Age, Armed) 
                                            VALUES (@IdTimelineinfo, @IdSubject, @Age, @Armed);";
                    await connection.ExecuteAsync(applyChanges, changes);
                }

                string changeToConfirmed = "UPDATE edithistory SET `Confirmed` = '1' WHERE (`IdEditHistory` = @IdEditHistory);";
                await connection.ExecuteAsync(changeToConfirmed, new {Ids.ToArray()[ActiveIdIndex].IdEditHistory});

                //var confirmedEditId = Ids.ToArray()[ActiveIdIndex].IdEditHistory;
                //Ids = Ids.Where(i => i.IdEditHistory != confirmedEditId).ToList();

            }         
        }
    }
}
