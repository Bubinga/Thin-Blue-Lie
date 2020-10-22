using DataAccessLibrary.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.DataModels
{
    [Table("edits")]
    public partial class Edits
    {
        public Edits()
        {
            EditMedia = new HashSet<EditMedia>();
        }

        [Key]
        public int IdEdits { get; set; }
        public int IdTimelineinfo { get; set; }
        [Required]
        [Column(TypeName = "char(10)")]
        public string Date { get; set; }
        public byte State { get; set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public string City { get; set; }
        public sbyte Misconduct { get; set; }
        public byte? Weapon { get; set; }
        [Required]
        [Column(TypeName = "mediumtext")]
        public string Context { get; set; }
        public byte Locked { get; set; }
        [Column(TypeName = "varchar(255)")]
        public string SubmittedBy { get; set; }
        public byte Confirmed { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey(nameof(IdTimelineinfo))]
        [InverseProperty(nameof(TimelineinfoFull.Edits))]
        public virtual TimelineinfoFull IdTimelineinfoNavigation { get; set; }
        [InverseProperty("IdEditsNavigation")]
        public virtual ICollection<EditMedia> EditMedia { get; set; }
        [ForeignKey(nameof(SubmittedBy))]
        [InverseProperty(nameof(Aspnetusers.Edits))]
        public virtual Aspnetusers SubmittedByNavigation { get; set; }
    }
}
