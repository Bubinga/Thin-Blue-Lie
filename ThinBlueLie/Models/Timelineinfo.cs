using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThinBlue
{
    public partial class Timelineinfo
    {        
        public int IdTimelineInfo { get; set; }
        [Required]
        [Column(TypeName = "CHAR(10)")]
        [MaxLength(10)]
        [DataType(DataType.Date)]
        public string Date { get; set; }
        [Required]
        [Column(TypeName = "TINYINT")]
        public int State { get; set; }
        [Required]
        [MaxLength(86)]
        [Column(TypeName = "VARCHAR(20)")]
        public string City { get; set; }
        [MaxLength(60)]
        [Column(TypeName = "VARCHAR(60)")]
        public string SubjectName { get; set; }
        [Required]
        [Column(TypeName = "TINYINT")]
        public int SubjectSex { get; set; }
        [Column(TypeName = "TINYINT")]
        public int SubjectRace { get; set; }
        [Column(TypeName = "TINYINT")]
        public int Armed { get; set; }
        [MaxLength(60)]
        [Column(TypeName = "VARCHAR(60)")]
        public string OfficerName { get; set; }
        [Column(TypeName = "TINYINT")]
        public int OfficerSex { get; set; }
        [Column(TypeName = "TINYINT")]
        public int OfficerRace { get; set; }
        [Column(TypeName = "TINYINT")]
        public int Misconduct { get; set; }
        [Column(TypeName = "TINYINT")]
        public int Weapon { get; set; }
        [Required]
        [Column(TypeName = "LONGTEXT")]
        public string Context { get; set; }
        //[Required]
        //[Column(TypeName = "TINYINT")]
        //public int Gore { get; set; }
        //[Required]
        //[Column(TypeName = "TINYINT")]
        //public int Source { get; set; }
        //[Column(TypeName = "VARCHAR(60)")]
        //public string Credit { get; set; }
        //[Column(TypeName = "VARCHAR(100)")]
        //public string VidLink { get; set; }
        [Column(TypeName = "VARCHAR(50)")]
        public string SubmittedBy { get; set; }
        [Column(TypeName = "TINYINT")]
        public int Locked { get; set; }
        [Column(TypeName = "TINYINT")]
        public int Verified { get; set; }

    }
}
