using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThinBlue
{
    public partial class Timelineinfo
    {
        public Timelineinfo()
        {
            Edits = new HashSet<Edits>();
            Flagged = new HashSet<Flagged>();
            Log = new HashSet<Log>();
            Media = new HashSet<Media>();
            TimelineinfoOfficer = new HashSet<TimelineinfoOfficer>();
            TimelineinfoSubject = new HashSet<TimelineinfoSubject>();
        }

        public int IdTimelineInfo { get; set; }
        [Required]
        [MaxLength(10)]
        [DataType(DataType.Date)]
        public string Date { get; set; }
        [Required]
        public byte State { get; set; }
        [Required]
        [MaxLength(86)]
        public string City { get; set; }
        public byte Misconduct { get; set; }
        public byte? Weapon { get; set; }
        public string Context { get; set; }
        public byte Locked { get; set; }
        public string SubmittedBy { get; set; }
        public byte Verified { get; set; }

        public virtual Aspnetusers SubmittedByNavigation { get; set; }
        public virtual ICollection<Edits> Edits { get; set; }
        public virtual ICollection<Flagged> Flagged { get; set; }
        public virtual ICollection<Log> Log { get; set; }
        public virtual ICollection<Media> Media { get; set; }
        public virtual ICollection<TimelineinfoOfficer> TimelineinfoOfficer { get; set; }
        public virtual ICollection<TimelineinfoSubject> TimelineinfoSubject { get; set; }
    }
}
