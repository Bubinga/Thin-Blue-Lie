using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLieB.Models
{
    public class BaseOfficer
    {
        public string? Name { get; set; }
        public byte Race { get; set; }
        public byte Sex { get; set; }
        public int? Age { get; set; }
        public int Misconduct { get; set; }
        public int? Weapon { get; set; }
    }
}
