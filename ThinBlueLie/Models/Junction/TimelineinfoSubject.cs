using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThinBlue
{
    public partial class TimelineinfoSubject
    {
        [Key]
        public int IdTimelineinfoSubject { get; set; }
        public int IdTimelineinfo { get; set; }
        public int IdSubject { get; set; }
        public byte Armed { get; set; }

        public virtual Subjects IdSubjectNavigation { get; set; }
        public virtual Timelineinfo IdTimelineinfoNavigation { get; set; }
    }
}
