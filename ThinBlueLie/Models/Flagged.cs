using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThinBlue
{
    public partial class Flagged
    {
        public int IdFlagged { get; set; }
        public int IdTimelineInfo { get; set; }
        [Required]
        public int FlagType { get; set; }
        [Required]
        public string Message { get; set; }
        public string UserId { get; set; }

        public virtual Timelineinfo IdTimelineInfoNavigation { get; set; }
        public virtual Aspnetusers User { get; set; }
    }
}
