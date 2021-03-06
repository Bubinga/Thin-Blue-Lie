using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLie.Helper.Validators
{
    public class IsTrueValidator : ValidationAttribute
    {
        /// <summary>
        /// Determines whether the specified value of the object is valid. 
        /// </summary>
        /// <returns>
        /// true if the specified value is valid; otherwise, false. 
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || (bool?)value == false)
            {
                return new ValidationResult( ErrorMessage, new[] { validationContext.MemberName });
            }
            else
            {
                return null;
            }
        }
    }
}
