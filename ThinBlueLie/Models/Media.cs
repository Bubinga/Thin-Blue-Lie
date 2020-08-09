using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThinBlue
{
    public partial class Media
    {
        public int IdMedia { get; set; }
        public int IdTimelineInfo { get; set; }
        [Required]
        public byte MediaType { get; set; }
        public string SourceFile { get; set; }
        [Required]
        public byte Gore { get; set; }
        [Required]
        public byte SourceFrom { get; set; }
        [Required]
        [MaxLength(250)]
        public string Blurb { get; set; }
        public string SubmittedBy { get; set; }

        public virtual Timelineinfo IdTimelineinfoNavigation { get; set; }
    }
}
