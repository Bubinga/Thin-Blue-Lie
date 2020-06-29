using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThinBlue
{
    public partial class Timelineinfo
    {
        
        public int IdTimelineInfo { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string City { get; set; }
        public string SubjectName { get; set; }
        public string SubjectSex { get; set; }
        public string SubjectRace { get; set; }
        public string Armed { get; set; }
        public string OfficerName { get; set; }
        public string OfficerSex { get; set; }
        public string OfficerRace { get; set; }
        public string Misconduct { get; set; }
        public string Weapon { get; set; }
        public string Context { get; set; }
        [Required]
        public string Gore { get; set; }
        public string VidLink { get; set; }
    }
}
