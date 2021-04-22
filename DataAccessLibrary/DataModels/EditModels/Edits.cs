using DataAccessLibrary.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DataAccessLibrary.Enums.TimelineinfoEnums;

namespace DataAccessLibrary.DataModels
{
    [Table("edits")]
    public partial class Edits {

        [Key]
        public int IdEdits { get; set; }
        public int IdEditHistory { get; set; }
        public int IdTimelineinfo { get; set; }
        public string Title { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public StateEnum State { get; set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public string City { get; set; }
        [Required]
        [Column(TypeName = "mediumtext")]
        public string Context { get; set; }
        public SupDataEnum SupData { get; set; }
        public byte Locked { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
