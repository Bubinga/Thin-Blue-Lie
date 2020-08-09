using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThinBlue
{
    public partial class Edits : Timelineinfo
    {
        public Edits()
        {
            EditMedia = new HashSet<EditMedia>();
        }

        public int IdEdits { get; set; }
        //[Required]
        //[Column(TypeName = "CHAR(10)")]
        //[MaxLength(10)]
        //[DataType(DataType.Date)]
        //public string Date { get; set; }
        //[Required]
        //[Column(TypeName = "TINYINT")]
        //public byte State { get; set; }
        //[Required]
        //[MaxLength(86)]
        //[Column(TypeName = "VARCHAR(20)")]
        //public string City { get; set; }
        //public byte Misconduct { get; set; }
        //public byte? Weapon { get; set; }
        //public string Context { get; set; }
        //public byte Locked { get; set; }
        //public string SubmittedBy { get; set; }
        public byte Confirmed { get; set; }

        public virtual ICollection<EditMedia> EditMedia { get; set; }
    }
}
