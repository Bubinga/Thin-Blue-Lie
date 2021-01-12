using DataAccessLibrary.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DataAccessLibrary.Enums.PrivilegeEnum;

namespace ThinBlueLie.Helper.Extensions
{
    public static class IdentityExtensions
    {
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
