using DataAccessLibrary.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.DataModels
{
    [Table("log")]
    public partial class Log
    {
        [Key]
        public int IdLog { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime TimeStamp { get; set; }
        public uint Action { get; set; }
        [Required]
        [Column(TypeName = "tinytext")]
        public string IpAddress { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string UserId { get; set; }
        public int? IdTimelineInfo { get; set; }

        [ForeignKey(nameof(IdTimelineInfo))]
        [InverseProperty(nameof(Timelineinfo.Log))]
        public virtual Timelineinfo IdTimelineInfoNavigation { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(Aspnetusers.Log))]
        public virtual Aspnetusers UserIdNavigation { get; set; }
    }
}
