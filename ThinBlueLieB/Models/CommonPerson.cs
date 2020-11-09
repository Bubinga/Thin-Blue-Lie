using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DataAccessLibrary.Enums.TimelineinfoEnums;

namespace ThinBlueLieB.Models
{
    public class CommonPerson
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public SexEnum Sex { get; set; }
        public RaceEnum Race { get; set; }
    }
}
