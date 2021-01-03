using DataAccessLibrary.DataModels;
using DataAccessLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLie.Models.ViewModels
{
    public class TimelineinfoFull
    {
        public Timelineinfo Timelineinfo { get; set; }
        public List<TimelineinfoOfficerShort> OfficerInfo { get; set; }
        public class TimelineinfoOfficerShort
        {
            public TimelineinfoEnums.MisconductEnum Misconduct { get; set; }
            public TimelineinfoEnums.WeaponEnum Weapon { get; set; }
        }
    }
}
