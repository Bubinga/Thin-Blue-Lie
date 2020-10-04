using DataAccessLibrary.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLieB.Helper.Validators
{
    public class MisconductValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var length = Enum.GetValues(typeof(TimelineinfoEnums.MisconductEnum)).Length;
            var misconductMax = Math.Pow(2, length);
            if ((int)value > misconductMax | (int)value == 0) //bad
            {
                return new ValidationResult("Remove select at least one Misconduct", new[] { validationContext.MemberName });
            }
            else
            {
                return null;
            }
        }

    }
}
