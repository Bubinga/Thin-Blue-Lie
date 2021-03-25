using DataAccessLibrary.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DataAccessLibrary.Enums.FlaggedEnums;

namespace DataAccessLibrary.DataModels
{
    [Table("flags")]
    public partial class Flags
    {
        [Key]
        public int IdFlags { get; set; }
        public int IdTimelineinfo { get; set; }
        [Required]
        public FlagTypeEnum? FlagType { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string Message { get; set; }
        [Column(TypeName = "varchar(255)")]
        public int? UserId { get; set; }
        public byte Status { get; set; }
        /// <summary>
        /// For use in FlagReview
        /// </summary>
        public string? EventTitle { get; set; }
        public DateTime Date { get; set; }
    }
}
