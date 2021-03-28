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
    public static partial class ClassExtensions
    {
        public static bool ContainsChange(this EditHistory editHistory)
        {
            if (!((editHistory?.Edits ?? 0 ) == 0) ||
                !((editHistory?.EditMedia ?? 0) == 0) ||
                !((editHistory?.Misconducts ?? 0) == 0))
            {
                return true;
            }
            return false;
        }

        public static bool PersonChange<T>(this T subject, T oldSubject) where T : CommonPerson
        {
            if ((subject?.Name != oldSubject?.Name) || (subject?.Race != oldSubject?.Race) ||
                (subject?.Sex != oldSubject?.Sex))
            {
                return true;
            }
            return false;
        }

        public static int AgeFromDOB(this DateTime DOB, DateTime? endDate)
        {
            if (endDate == null)
                endDate = DateTime.Today;
            DateTime now = (DateTime)endDate;
            int age = now.Year - DOB.Year;
            if (DOB > now.AddYears(-age)) age--;
            return age;
        }

        public static void SetAgeFromDOB<T>(this List<T> People, DateTime? date) where T : CommonPerson
        {
            foreach (var person in People)
            {
                person.Age = person.DOB.AgeFromDOB(date);            
            }
        }
    }
}
