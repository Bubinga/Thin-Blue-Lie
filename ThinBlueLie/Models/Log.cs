using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlue
{
    public class Log
    {
        [Key]
        public int IdLog { get; set; }
        public string TimeStamp { get; set; }
        public int Action { get; set; }
        public string IpAddress { get; set; }
        public string IdUser { get; set; }
        public int IdTimelineinfo { get; set; }
    }
}
