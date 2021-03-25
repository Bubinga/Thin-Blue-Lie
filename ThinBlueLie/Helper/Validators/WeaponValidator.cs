using static DataAccessLibrary.Enums.TimelineinfoEnums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLie.Helper.Validators
{
    public class WeaponValidator : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public WeaponValidator(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            var weapons = Array.ConvertAll((int[])value, b => (short)b).Cast<WeaponEnum>().ToArray();

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
                throw new ArgumentException("Property with this name not found");

            var Misconducts = Array.ConvertAll((int[])property.GetValue(validationContext.ObjectInstance), b => (short)b).Cast<MisconductEnum>().ToArray();

            if (Misconducts.Contains(MisconductEnum.Force) && (weapons?.Length ?? 0) == 0)
                return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
            else
                return null;
        }
    }
}
