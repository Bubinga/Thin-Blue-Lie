using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlue
{
    public class Junc_Timelineinfo_Subject
    {
        [Key]
        public int IdLinkTimelineinfo_Subject { get; set; }
        public int IdTimelineinfo { get; set; }
        public int IdSubject { get; set; }
        public int Armed { get; set; }
    }
}
