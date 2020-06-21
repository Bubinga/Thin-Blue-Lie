using System;
using System.Collections.Generic;

namespace DataAccessLibrary.thinblue
{
    public partial class Timelineinfo
    {
        public int IdTimelineInfo { get; set; }
        public string Date { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string SubjectName { get; set; }
        public string SubjectSex { get; set; }
        public string SubjectRace { get; set; }
        public string Armed { get; set; }
        public string OfficerName { get; set; }
        public string OfficerSex { get; set; }
        public string OfficerRace { get; set; }
        public string Misconduct { get; set; }
        public string Weapon { get; set; }
        public string Context { get; set; }
        public string Gore { get; set; }
        public string VidLink { get; set; }
    }
}
