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
        public byte Race { get; set; }
        public byte Sex { get; set; }

        [InverseProperty("IdSubjectNavigation")]
        public virtual ICollection<TimelineinfoSubject> TimelineinfoSubject { get; set; }
    }
}
