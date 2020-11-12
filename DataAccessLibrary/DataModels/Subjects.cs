using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.DataModels
{
    [Table("subjects")]
    public partial class Subjects
    {
        public Subjects()
        {
            TimelineinfoSubject = new HashSet<TimelineinfoSubject>();
        }

        [Key]
        public int IdSubject { get; set; }
        [Column(TypeName = "varchar(60)")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The Subject's Race field is required")]
        public byte Race { get; set; }
        [Required(ErrorMessage = "The Subject's Sex field is required")]
        public byte Sex { get; set; }
        public string Image { get; set; }
        public byte Local { get; set; }

        [InverseProperty("IdSubjectNavigation")]
        public virtual ICollection<TimelineinfoSubject> TimelineinfoSubject { get; set; }
    }
}
