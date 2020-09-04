using System.ComponentModel.DataAnnotations;

namespace ThinBlueLieB.Models
{
    public class ViewSubject
    {
        public int IdSubject { get; set; }
        public int ListIndex { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage = "The Subject's Race field is required")]
        public byte Race { get; set; }
        [Required(ErrorMessage = "The Subject's Sex field is required")]
        public byte Sex { get; set; }
        public int? Age { get; set; }
        public bool SameAs { get; set; }
        public bool Armed { get; set; }
    }
}
