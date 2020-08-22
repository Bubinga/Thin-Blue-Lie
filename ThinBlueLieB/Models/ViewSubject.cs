using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLieB.Models
{
    public class ViewSubject
    {
        public int IdSubject { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage = "The Subject's Race field is required")]
        public byte Race { get; set; }
        [Required(ErrorMessage = "The Subject's Sex field is required")]
        public byte Sex { get; set; }
        public bool SameAs { get; set; }
        public bool Armed { get; set; }
    }
}
