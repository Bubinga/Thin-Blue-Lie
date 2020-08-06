using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlue
{
    public class Junc_Timelineinfo_Officer
    {
        [Key]
        public int IdLinkTimelineinfo_Officer { get; set; }
        public int IdTimelineinfo { get; set; }
        public int IdOfficer { get; set; }
    }
}
