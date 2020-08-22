using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.DataModels
{
    [Table("timelineinfo_officer")]
    public partial class TimelineinfoOfficer
    {
        [Key]
        [Column("IdTimelineinfo_Officer")]
        public int IdTimelineinfoOfficer { get; set; }
        public int IdTimelineinfo { get; set; }
        public int IdOfficer { get; set; }
        public int Misconduct { get; set; }
        public int? Weapon { get; set; }

        [ForeignKey(nameof(IdOfficer))]
        [InverseProperty(nameof(Officers.TimelineinfoOfficer))]
        public virtual Officers IdOfficerNavigation { get; set; }
        [ForeignKey(nameof(IdTimelineinfo))]
        [InverseProperty(nameof(Timelineinfo.TimelineinfoOfficer))]
        public virtual Timelineinfo IdTimelineinfoNavigation { get; set; }
    }
}
