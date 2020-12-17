using DataAccessLibrary.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DataAccessLibrary.Enums.MediaEnums;

namespace DataAccessLibrary.DataModels
{
    [Table("editmedia")]
    public partial class EditMedia
    {
        [Key]
        public int IdEditMedia { get; set; }
        public int IdEditHistory { get; set; }
        public int IdTimelineinfo { get; set; }
        public int IdMedia { get; set; }
        /// <summary>
        /// For Converting to ViewMedia
        /// </summary>
        public bool Processed { get; set; }
        public short Rank { get; set; }
        public MediaTypeEnum MediaType { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string SourcePath { get; set; }
        public string Thumbnail { get; set; }
        public byte Gore { get; set; }
        public SourceFromEnum SourceFrom { get; set; }
        [Required]
        [Column(TypeName = "tinytext")]
        public string Blurb { get; set; }
        public string Credit { get; set; }
        [Required]
        [Column(TypeName = "varchar(255)")]
        public int? SubmittedBy { get; set; }
        public DateTime Timestamp { get; set; }
        public byte Action { get; set; }

        //[ForeignKey(nameof(IdEdits))]
        //[InverseProperty(nameof(Edits.EditMedia))]
        //public virtual Edits IdEditsNavigation { get; set; }
        //[ForeignKey(nameof(SubmittedBy))]
        //[InverseProperty(nameof(Aspnetusers.EditMedia))]
        //public virtual Aspnetusers SubmittedByNavigation { get; set; }
    }
}
