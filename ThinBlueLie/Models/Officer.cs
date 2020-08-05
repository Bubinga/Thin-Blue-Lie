using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlue
{
    public class Officer
    {
        [Key]
        public int IdOfficer { get; set; }
        public string Name { get; set; }
        public int Race { get; set; }
        public int Sex { get; set; }
    }
}
