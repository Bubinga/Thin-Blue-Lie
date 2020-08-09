using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThinBlue
{
    public partial class Timelineinfo
    {
        public Timelineinfo()
        {
            Media = new HashSet<Media>();
            TimelineinfoOfficer = new HashSet<TimelineinfoOfficer>();
            TimelineinfoSubject = new HashSet<TimelineinfoSubject>();
        }

        public int IdTimelineInfo { get; set; }
        [Required]
        [Column(TypeName = "CHAR(10)")]
        [MaxLength(10)]
        [DataType(DataType.Date)]
        public string Date { get; set; }
        [Required]
        [Column(TypeName = "TINYINT")]
        public byte State { get; set; }
        [Required]
        [MaxLength(86)]
        [Column(TypeName = "VARCHAR(20)")]
        public string City { get; set; }
        public byte Misconduct { get; set; }
        public byte? Weapon { get; set; }
        public string Context { get; set; }
        public byte Locked { get; set; }
        public string SubmittedBy { get; set; }
        public byte Verified { get; set; }

        public virtual ICollection<Media> Media { get; set; }
        public virtual ICollection<TimelineinfoOfficer> TimelineinfoOfficer { get; set; }
        public virtual ICollection<TimelineinfoSubject> TimelineinfoSubject { get; set; }
    }
}
