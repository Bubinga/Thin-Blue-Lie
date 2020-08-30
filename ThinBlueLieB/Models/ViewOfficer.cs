using System.ComponentModel.DataAnnotations;

namespace ThinBlueLieB.Models
{
    public class ViewOfficer
    {
        public int IdOfficer { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage = "The Officer's Race field is required")]
        public byte Race { get; set; }
        [Required(ErrorMessage = "The Officer's Sex field is required")]
        public byte Sex { get; set; }
        public int? Age { get; set; }
        public int Misconduct { get; set; }
        public int Weapon { get; set; }
        public bool SameAs { get; set; }
    }
}
