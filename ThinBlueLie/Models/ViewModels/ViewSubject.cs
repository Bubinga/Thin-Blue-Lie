using DataAccessLibrary.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ThinBlueLie.Models
{
    public class ViewSubject
    {
        public int IdSubject { get; set; }
        public int Rank { get; set; }
        [RegularExpression("(?i)^(?:(?![×Þß÷þø])[-'a-zÀ-ÿ ])+$", ErrorMessage = "Enter only letters")]
        [MaxLength(60, ErrorMessage = "Please enter less than 60 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The Subject's Race field is required")]
        public TimelineinfoEnums.RaceEnum? Race { get; set; }
        [Required(ErrorMessage = "The Subject's Sex field is required")]
        public TimelineinfoEnums.SexEnum? Sex { get; set; }
        public string? Image { get; set; }
        public byte Local { get; set; }
        [Range(0, 130,
        ErrorMessage = "Age must be between {1} and {2}.")]
        public int? Age { get; set; }
        public DateTime? DOB { get; set; }
        public int? SameAsId { get; set; }
    }
}
