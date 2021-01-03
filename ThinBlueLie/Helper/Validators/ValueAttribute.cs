using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLie.Helper.Validators
{
    public class ValueAttribute : ValidationAttribute
    {
        public ValueAttribute(int value)
        {
            Value = value;
        }

        public int Value { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return ValidationResult.Success;
        }
    }
}

