using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlue
{ 
    public class Subject
    {
        [Key]
        public int IdSubject { get; set; }
       // [Required(ErrorMessage = "The Subject's Name field is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The Subject's Race field is required")]
        public int Race { get; set; }
        [Required(ErrorMessage = "The Subject's Sex field is required")]
        public int Sex { get; set; }       
    }
}
