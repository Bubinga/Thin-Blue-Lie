using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThinBlue
{
    public partial class Subjects
    {
        public Subjects()
        {
            TimelineinfoSubject = new HashSet<TimelineinfoSubject>();
        }
        [Key]
        public int IdSubject { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage = "The Subject's Race field is required")]
        public byte Race { get; set; }
        [Required(ErrorMessage = "The Subject's Sex field is required")]
        public byte Sex { get; set; }

        public virtual ICollection<TimelineinfoSubject> TimelineinfoSubject { get; set; }
    }
}
