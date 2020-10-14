using DataAccessLibrary.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.DataModels
{
    [Table("flagged")]
    public partial class Flagged
    {
        [Key]
        public int IdFlagged { get; set; }
        public int IdTimelineinfo { get; set; }
        public uint FlagType { get; set; }
        [Column(TypeName = "text")]
        public string Message { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string UserId { get; set; }

        [ForeignKey(nameof(IdTimelineinfo))]
        [InverseProperty(nameof(TimelineinfoFull.Flagged))]
        public virtual TimelineinfoFull IdTimelineinfoNavigation { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(Aspnetusers.Flagged))]
        public virtual Aspnetusers UserIdNavigation { get; set; }
    }
}
