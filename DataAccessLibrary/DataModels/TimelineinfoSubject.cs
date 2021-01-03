using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.DataModels
{
    [Table("timelineinfo_subject")]
    public partial class TimelineinfoSubject
    {
        [Key]
        [Column("IdTimelineinfo_Subject")]
        public int IdTimelineinfoSubject { get; set; }
        public int IdTimelineinfo { get; set; }
        public int IdSubject { get; set; }
        public byte Armed { get; set; }
        public byte? Age { get; set; }
    }
}
