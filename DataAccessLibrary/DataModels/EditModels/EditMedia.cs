using DataAccessLibrary.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.DataModels
{
    [Table("editmedia")]
    public partial class EditMedia
    {
        [Key]
        public int IdEditMedia { get; set; }
        public int IdEdits { get; set; }
        public short Rank { get; set; }
        public byte MediaType { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string SourcePath { get; set; }
        public byte Gore { get; set; }
        public byte SourceFrom { get; set; }
        [Required]
        [Column(TypeName = "tinytext")]
        public string Blurb { get; set; }
        public string Credit { get; set; }
        [Required]
        [Column(TypeName = "varchar(255)")]
        public string SubmittedBy { get; set; }
        public DateTime Timestamp { get; set; }
        public byte Action { get; set; }

        [ForeignKey(nameof(IdEdits))]
        [InverseProperty(nameof(Edits.EditMedia))]
        public virtual Edits IdEditsNavigation { get; set; }
        [ForeignKey(nameof(SubmittedBy))]
        [InverseProperty(nameof(Aspnetusers.EditMedia))]
        public virtual Aspnetusers SubmittedByNavigation { get; set; }
    }
}
