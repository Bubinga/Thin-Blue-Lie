using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLieB.Helper.Validators
{
    public class ListCountValidator : ValidationAttribute
    {
        public ListCountValidator(int minimum, int maximum, string property)
        {
            Maximum = maximum;
            Minimum = minimum;
            Property = property;
        }

        public int Maximum { get; }
        public int Minimum { get; }
        public string Property { get; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int Value = new List<object>((IEnumerable<object>)value).Count;
            if ((Value <= Maximum && Value >= Minimum) == false)
            {
                return new ValidationResult($"Enter between {Minimum} and {Maximum} {Property}", new[] { validationContext.MemberName });
            }
            else
            {
                return null;
            }
        }
    }
}
