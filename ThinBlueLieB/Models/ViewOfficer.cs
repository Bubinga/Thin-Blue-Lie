using System.ComponentModel.DataAnnotations;

namespace ThinBlueLieB.Models
{
    public class ViewOfficer
    {
        public int IdOfficer { get; set; }
        public int ListIndex { get; set; }
        [RegularExpression("(?i)^(?:(?![×Þß÷þø])[-'a-zÀ-ÿ ])+$", ErrorMessage = "Enter only letters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The Officer's Race field is required")]
        public byte Race { get; set; }
        [Required(ErrorMessage = "The Officer's Sex field is required")]
        public byte Sex { get; set; }
        [Range(0, 130,
        ErrorMessage = "Age must be between {1} and {2}.")]
        public int? Age { get; set; }
        public int[]? Misconduct { get; set; }
        public int[]? Weapon { get; set; }
        public bool SameAs { get; set; }
    }
}
