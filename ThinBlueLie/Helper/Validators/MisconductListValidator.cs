using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLie.Models;

namespace ThinBlueLie.Helper.Validators
{
    public class MisconductListValidator : ValidationAttribute
    {
        //public MisconductListValidator(List<ViewSubject> subjects, List<ViewOfficer> officers)
        //{
        //    Subjects = subjects;
        //    Officers = officers;
        //}

        //public List<ViewSubject> Subjects { get; }
        //public List<ViewOfficer> Officers { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            List<ViewMisconduct> misconducts = (List<ViewMisconduct>)value;
            var Subjects = ((SubmitModel)validationContext.ObjectInstance).Subjects;
            var Officers = ((SubmitModel)validationContext.ObjectInstance).Officers;
            List<string> names = new();
            foreach (var subject in Subjects)
            {
                if (misconducts.Select(s => s.Subject).Contains(subject) == false)
                    names.Add(subject.Name);
            }
            foreach (var officer in Officers)
            {
                if (misconducts.Select(s => s.Officer).Contains(officer) == false)
                    names.Add(officer.Name);
            }
            if (names.Count > 0)
            {
                var namelist = string.Join(", ", names.ToArray());
                var isare = names.Count > 1 ? "are" : "is";
                return new ValidationResult($"{namelist} {isare} missing a misconduct", new[] { validationContext.MemberName });
            }
            return null;
        }
    }
}
