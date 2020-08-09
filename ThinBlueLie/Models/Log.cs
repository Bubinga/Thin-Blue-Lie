using System;
using System.Collections.Generic;

namespace ThinBlue
{
    public partial class Log
    {
        public int IdLog { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public int Action { get; set; }
        public string IpAddress { get; set; }
        public string UserId { get; set; }
        public int? IdTimelineInfo { get; set; }

        public virtual Timelineinfo IdTimelineInfoNavigation { get; set; }
        public virtual Aspnetusers User { get; set; }
    }
}
