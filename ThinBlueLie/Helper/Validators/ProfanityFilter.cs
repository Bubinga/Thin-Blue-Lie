using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLie.Helper.Algorithms
{
    public class ProfanityFilter : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return new ValidationResult("Remove all profanity from your file name", new[] { validationContext.MemberName });
        }
    }
}
