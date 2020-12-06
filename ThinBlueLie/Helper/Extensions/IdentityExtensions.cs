using DataAccessLibrary.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DataAccessLibrary.Enums.PrivledgeEnum;

namespace ThinBlueLie.Helper.Extensions
{
    public static class IdentityExtensions
    {
        public static bool RepAuthorizer(this ApplicationUser User, Privledges Privledge)
        {
            if (User.Reputation >= (int)Privledge)
                return true;
            else
                return false;
        }
        public static Privledges NextPrivledge(this ApplicationUser User)
        {
            int Rep = User.Reputation;
            foreach (int privledge in Enum.GetValues(typeof(Privledges)))
            {
                if (Rep < privledge)
                {
                    return (Privledges)privledge;
                }
            }
            return Privledges.CreateEditFlag;
        }
        public static Privledges CurrentPrivledge(this ApplicationUser User)
        {
            int Rep = User.Reputation;
            foreach (int privledge in ((Privledges[])Enum.GetValues(typeof(Privledges))).OrderByDescending(x => x))
            {
                if (Rep > privledge)
                {
                    return (Privledges)privledge;
                }
            }
            return Privledges.CreateEditFlag;            
        }
    }
}
