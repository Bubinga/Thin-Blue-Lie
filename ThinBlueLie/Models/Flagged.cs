using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlue
{
    public class Flagged
    {
        [Key]
        public int IdFlagged { get; set; }
        public int IdTimelineInfo { get; set; }
        public int Type { get; set; }
        public string Message { get; set; }
    }
}
