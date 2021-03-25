using DataAccessLibrary.UserModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using static DataAccessLibrary.Enums.ReputationEnum;

namespace ThinBlueLie.Helper.Extensions
{
    public static class IdentityExtensions
    {
        public static async Task ChangeReputation(this UserManager<ApplicationUser> userManager, ReputationChangeEnum change, int userId)
        {
            if (userId == 0) return; //If it's not community owned
            var User = await userManager.FindByIdAsync(Convert.ToString(userId));
            User.Reputation += (int)change;
            await userManager.UpdateAsync(User);
            Serilog.Log.Information("User {UserId} {ReputationChange} rep for {RepChangeReason}",
                       userId, ((int)change).ToString("+#.##;-#.##;0"), change.ToString());
        }
        public static bool RepAuthorizer(this ApplicationUser User, Privileges Privilege)
        {
            if (User.Reputation >= (int)Privilege)
                return true;
            else
                return false;
        }
        public static Privileges NextPrivilege(this ApplicationUser User)
        {
            int Rep = User.Reputation;
            foreach (int privilege in Enum.GetValues(typeof(Privileges)))
            {
                if (Rep < privilege)
                {
                    return (Privileges)privilege;
                }
            }
            return Privileges.CreateEditFlag;
        }
        public static Privileges CurrentPrivilege(this ApplicationUser User)
        {
            int Rep = User.Reputation;
            foreach (int privilege in ((Privileges[])Enum.GetValues(typeof(Privileges))).OrderByDescending(x => x))
            {
                if (Rep > privilege)
                {
                    return (Privileges)privilege;
                }
            }
            return Privileges.CreateEditFlag;            
        }
    }
}
