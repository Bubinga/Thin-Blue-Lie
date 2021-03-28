using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.DataModels
{
    public class Misconducts
    {
        public int IdTimelineinfo { get; set; }
        public int IdOfficer { get; set; }
        public int IdSubject { get; set; }
        public int Misconduct { get; set; }
        public int? Weapon { get; set; }
        public byte Armed { get; set; }
    }
}