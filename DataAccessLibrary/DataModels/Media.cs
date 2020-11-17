using DataAccessLibrary.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.DataModels
{
    [Table("media")]
    public partial class Media
    {
        [Key]
        public int IdMedia { get; set; }
        public int IdTimelineinfo { get; set; }
        public byte MediaType { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string SourcePath { get; set; } //Either the provided link or a pointer to the location where the uploaded file is stored
        public byte Gore { get; set; }
        public byte SourceFrom { get; set; }
        [Required]
        [MaxLength(250)]
        [Column(TypeName = "tinytext")]
        public string Blurb { get; set; }
        public string Credit { get; set; }
        [Column(TypeName = "varchar(255)")]
        public int? SubmittedBy { get; set; }
        public short Rank { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey(nameof(IdTimelineinfo))]
        [InverseProperty(nameof(TimelineinfoFull.Media))]
        public virtual TimelineinfoFull IdTimelineinfoNavigation { get; set; }
        [ForeignKey(nameof(SubmittedBy))]
        [InverseProperty(nameof(Aspnetusers.Media))]
        public virtual Aspnetusers SubmittedByNavigation { get; set; }
    }
}
