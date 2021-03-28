using Dapper;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.DataModels;
using DataAccessLibrary.DataModels.EditModels;
using DataAccessLibrary.UserModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using MySqlConnector;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLie.Models.ViewModels;
using ThinBlueLie.Searches;
using ThinBlueLie.Helper.Extensions;
using static DataAccessLibrary.Enums.EditEnums;
using static ThinBlueLie.Helper.ConfigHelper;
using static ThinBlueLie.Searches.SearchClasses;
using DataAccessLibrary.Enums;

namespace ThinBlueLie.Pages.Account
{
    public partial class EditReview
    {
        public EditReviewModel versions = new();
        [Inject]
        public UserManager<ApplicationUser> UserManager { get; set; }
        [Inject]
        public RoleManager<ApplicationRole> RoleManager { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> AuthState { get; set; }
        private AuthenticationState userState;
        ApplicationUser User;
        public int PendingEditsCount;
        public List<FirstLoadEditHistory> Ids;
        public int ActiveIdIndex;
        public bool Loading = true;
        public bool EditsOnly = true;
        public List<EditHistory> EditChanges = new();
        public List<EditReviewModel> Edits = new();
        [Inject]
        SearchesEditReview Review { get; set; }
        [Inject]
        IDataAccess Data { get; set; }
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
            if (Ids == null || Ids.Any() is false)
            {
                PendingEditsCount = 0;
                Loading = false;
            }
            else
            {
                //get number of all the unconfirmed edits that user can access
                var firstId = Ids?.FirstOrDefault()?.IdTimelineinfo;
                if (firstId != 0 || firstId != null) //firstId will never be zero other than if ids is empty
                {
                    var change = await Review.GetEditFromId(Ids.FirstOrDefault());
                    Edits.Add(change.Item1);
                    EditChanges.Add(change.Item2);
                    ActiveIdIndex = 0;
                }
            }
            PendingEditsCount = Ids.Count;
            Loading = false;
        }
        public async Task GetNextEdit()
        {
            if (PendingEditsCount >= 1)
                return;
            Loading = true;
            //if it hasnt already been loaded
            if (Edits.Count - 1 <= ActiveIdIndex)
            {
                var change = await Review.GetEditFromId(Ids[ActiveIdIndex == PendingEditsCount - 1 ? 0 : ActiveIdIndex + 1]);
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
            if (PendingEditsCount is 0)
                return;
            if (ActiveIdIndex != 0)
                ActiveIdIndex--;
            StateHasChanged();
        }

        public const int minimumAcceptVotes = 5;
        public const int minimumDenyVotes = -4;
        //TODO make only able to vote once.

        public async Task VoteEdit(short voteAmount)
        {
            var Vote = Ids[ActiveIdIndex].Vote;
            if (Vote != voteAmount)
            {
                string RejectSql = "INSERT INTO edit_votes (`IdEditHistory`, `UserId`, `Vote`) " +
                        "VALUES(@IdEditHistory, @UserId, @Vote) AS new " +
                        "ON DUPLICATE KEY UPDATE " +
                        "Vote = new.Vote; " +
                    "SELECT e.Vote FROM edit_votes e where e.IdEditHistory = @IdEditHistory;";
                IEnumerable<int> rows;
                using (IDbConnection connection = new MySqlConnection(GetConnectionString()))
                {
                    rows = await connection.QueryAsync<int>(RejectSql, new { Ids[ActiveIdIndex].IdEditHistory, UserId = User.Id, Vote = voteAmount });
                }
                Ids[ActiveIdIndex].Vote = voteAmount;
                await GetEditTask(rows);
            }
        }

        public async Task GetEditTask(IEnumerable<int> rows)
        {
            bool DidSomething = false;
            if (rows.Sum() <= minimumDenyVotes)
            {
                await DenyEdit();
                DidSomething = true;
            }
            else if (rows.Sum() >= minimumAcceptVotes)
            {
                await MergeEdit();
                DidSomething = true;
            }

            if (DidSomething)
            {
                //var id = Ids[ActiveIdIndex].IdEditHistory;
                Edits.RemoveAt(ActiveIdIndex);
                EditChanges.RemoveAt(ActiveIdIndex);
                var x = Ids.ToList();
                x.RemoveAt(ActiveIdIndex);
                Ids = x;
                ActiveIdIndex--;
                PendingEditsCount--;
            }
        }
        public async Task DenyEdit()
        {
            string DenyEditSql = "UPDATE edithistory SET `Confirmed` = '2' WHERE (`IdEditHistory` = @id);";
            await Data.SaveData(DenyEditSql, new { id = Ids[ActiveIdIndex].IdEditHistory });
        }

        public async Task MergeEdit()
        {
            var change = EditChanges[ActiveIdIndex];
            var newInfo = Edits[ActiveIdIndex].New;

            //TODO change system so that I don't have to query this information twice
            using (IDbConnection connection = new MySqlConnection(GetConnectionString()))
            {
                if (change.Edits == 1)
                {
                    string EditChangedQuery = "INSERT INTO timelineinfo(`IdTimelineinfo`, `Title`, `Date`, `State`, `City`, `Context`, `Locked`, `Owner`)" +
                                                   $"VALUES(@IdTimelineinfo, @Title, @Date, @State, @City, @context, @Locked, '{Ids[ActiveIdIndex].SubmittedBy}')" +
                                                "ON DUPLICATE KEY UPDATE " +
                                                   "`Title` = @Title, `Date` = @Date, `State` = @State, " +
                                                   "`City` = @City, `Context` = @Context, " +
                                                   $"`Locked` = @Locked, `Owner` = '{Ids[ActiveIdIndex].SubmittedBy}';";
                    await connection.ExecuteAsync(EditChangedQuery, newInfo.Data);
                }
                if (change.EditMedia == 1)
                {
                    string MediaChangedQuery = @"Select * From editmedia m Where m.IdEditHistory = @id;";
                    var changesToMedia = await Data.LoadData<EditMedia, dynamic>(MediaChangedQuery, new { id = change.IdEditHistory });
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
                    List<EditOfficer> changes = (List<EditOfficer>)await connection.QueryAsync<EditOfficer>(GetOfficerChanges, new { id = change.IdEditHistory });
                    //Small enough file and not editted enough for optimization
                    var firstAction = (EditActions)changes.FirstOrDefault().Action;
                    if (changes.Count > 1 || firstAction == EditActions.Addition)
                    {
                        string AddOfficer = "INSERT INTO officers (`IdOfficer`, `Name`, `Race`, `Sex`, `Image`, `Local`) " +
                                              "VALUES(@IdOfficer, @Name, @Race, @Sex, @Image, @Local);";
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
                if (change.Misconducts == 1)
                {
                    string getTOChanges = "Select * from edits_misconducts e Where e.IdEditHistory = @id";
                    var changes = await connection.QueryAsync<EditMisconducts>(getTOChanges, new { id = change.IdEditHistory });
                    string deleteEverything = "Delete from misconducts m where m.IdTimelineinfo = @IdTimelineinfo;";
                    await connection.ExecuteAsync(deleteEverything, changes);
                    string applyChanges = "INSERT INTO misconducts (`IdTimelineinfo`, `IdOfficer`, `IdSubject`, `SubjectAge`, `OfficerAge`, `Misconduct`, `Weapon`, `Armed`) "+ 
                                                "VALUES (@IdTimelineinfo, @IdOfficer, @IdSubject, @SubjectAge, @OfficerAge, @Misconduct, @Weapon, @Armed`);";
                    await connection.ExecuteAsync(applyChanges, changes);
                }

                string changeToConfirmed = "UPDATE edithistory SET `Confirmed` = '1' WHERE (`IdEditHistory` = @IdEditHistory);";
                await connection.ExecuteAsync(changeToConfirmed, new { Ids[ActiveIdIndex].IdEditHistory });
                Serilog.Log.Information("Merged Edit for Event {id}", newInfo.Data.IdTimelineinfo);
                //var confirmedEditId = Ids[ActiveIdIndex].IdEditHistory;
                //Ids = Ids.Where(i => i.IdEditHistory != confirmedEditId).ToList();

            }
            var ownerId = EditChanges[ActiveIdIndex].SubmittedBy;
            await UserManager.ChangeReputation(ReputationEnum.ReputationChangeEnum.EditAccepted, ownerId);
        }
    }
}
