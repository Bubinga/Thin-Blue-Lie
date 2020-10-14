using DataAccessLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLieB.ViewModels
{
    public class DisplayOfficer
    {
        public string Name { get; set; }
        public TimelineinfoEnums.RaceEnum? Race { get; set; }
        public TimelineinfoEnums.SexEnum? Sex { get; set; }
        public int? Age { get; set; }
        public int[]? Misconduct { get; set; }
        public int[]? Weapon { get; set; }
    }
}
