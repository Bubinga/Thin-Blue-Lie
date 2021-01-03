using DataAccessLibrary.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.DataModels
{
    [Table("flags")]
    public partial class Flags
    {
        [Key]
        public int IdFlagged { get; set; }
        public int IdTimelineinfo { get; set; }
        public uint FlagType { get; set; }
        [Column(TypeName = "text")]
        public string Message { get; set; }
        [Column(TypeName = "varchar(255)")]
        public int? UserId { get; set; }

    }
}
