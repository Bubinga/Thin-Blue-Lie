using DataAccessLibrary.DataModels;
using DataAccessLibrary.DataModels.EditModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ThinBlueLie.Models;

namespace ThinBlueLie.Helper.Extensions
{
    public static class ClassExtensions
    {
        public static bool ContainsChange(this EditHistory editHistory)
        {
            if (!((editHistory?.Edits ?? 0 ) == 0) ||
                !((editHistory?.EditMedia ?? 0) == 0) ||
                !((editHistory?.Timelineinfo_Subject ?? 0) == 0) ||
                !((editHistory?.Timelineinfo_Officer ?? 0) == 0))
            {
                return true;
            }
            return false;
        }

        public static bool JunctionChange<T>(this T subject, T oldSubject) where T : CommonPerson
        {
            if ((subject?.Name != oldSubject?.Name) || (subject?.Race != oldSubject?.Race) ||
                        (subject?.Sex != oldSubject?.Sex) || (subject?.Image != oldSubject?.Image) ||
                        (subject?.Local != oldSubject?.Local))
            {
                return true;
            }
            return false;
        }
    }
}
