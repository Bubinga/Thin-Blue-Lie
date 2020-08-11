using DataAccessLibrary.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.DataModels
{
    [Table("timelineinfo")]
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

        [Key]
        public int IdTimelineInfo { get; set; }
        [Required]       
        [MaxLength(10)]
        [DataType(DataType.Date)]
        [Column(TypeName = "char(10)")]
        public string Date { get; set; }
        public byte State { get; set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public string City { get; set; }
        public byte Misconduct { get; set; }
        public byte? Weapon { get; set; }
        [Required]
        [Column(TypeName = "mediumtext")]
        public string Context { get; set; }
        public byte Locked { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string SubmittedBy { get; set; }
        public byte Verified { get; set; }

        [InverseProperty("IdTimelineInfoNavigation")]
        public virtual ICollection<Edits> Edits { get; set; }
        [InverseProperty("IdTimelineInfoNavigation")]
        public virtual ICollection<Flagged> Flagged { get; set; }
        [InverseProperty("IdTimelineInfoNavigation")]
        public virtual ICollection<Log> Log { get; set; }
        [InverseProperty("IdTimelineinfoNavigation")]
        public virtual ICollection<Media> Media { get; set; }
        [InverseProperty("IdTimelineinfoNavigation")]
        public virtual ICollection<TimelineinfoOfficer> TimelineinfoOfficer { get; set; }
        [InverseProperty("IdTimelineinfoNavigation")]
        public virtual ICollection<TimelineinfoSubject> TimelineinfoSubject { get; set; }
        [ForeignKey(nameof(SubmittedBy))]
        [InverseProperty(nameof(Aspnetusers.Timelineinfo))]
        public virtual Aspnetusers SubmittedByNavigation { get; set; }
    }
}
