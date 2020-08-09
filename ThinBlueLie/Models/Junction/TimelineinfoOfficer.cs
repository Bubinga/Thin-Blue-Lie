using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThinBlue
{
    public partial class TimelineinfoOfficer
    {
        [Key]
        public int IdTimelineinfoOfficer { get; set; }
        public int IdTimelineinfo { get; set; }
        public int IdOfficer { get; set; }

        public virtual Officers IdOfficerNavigation { get; set; }
        public virtual Timelineinfo IdTimelineinfoNavigation { get; set; }
    }
}
