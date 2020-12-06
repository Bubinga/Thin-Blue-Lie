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
            int[] values = (int[])value;
            if (values.Sum() > misconductMax | values.Sum() == 0) //bad
            {
                return new ValidationResult("Select at least one Misconduct", new[] { validationContext.MemberName });
            }
            else
            {
                return null;
            }
        }

    }
}
