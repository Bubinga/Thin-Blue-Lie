using DataAccessLibrary.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using ThinBlueLie.Helper.Validators;

namespace ThinBlueLie.Models
{
    public class ViewOfficer
    {               
        public int IdOfficer { get; set; }
        public int Rank { get; set; }
        [RegularExpression("(?i)^(?:(?![×Þß÷þø])[-'a-zÀ-ÿ ])+$", ErrorMessage = "Enter only letters")]
        [MaxLength(60, ErrorMessage = "Please enter less than 60 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The Officer's Race field is required")]
        public TimelineinfoEnums.RaceEnum? Race { get; set; }
        [Required(ErrorMessage = "The Officer's Sex field is required")]
        public TimelineinfoEnums.SexEnum? Sex { get; set; }
        public string Image { get; set; }
        public byte Local { get; set; }
        [Range(0, 130,
        ErrorMessage = "Age must be between {1} and {2}.")]
        public int? Age { get; set; }
        [Required]
        [MisconductValidator]
        public int[]? Misconduct { get; set; }
        public int[]? Weapon { get; set; }
        public int? SameAsId { get; set; }
    }
}
