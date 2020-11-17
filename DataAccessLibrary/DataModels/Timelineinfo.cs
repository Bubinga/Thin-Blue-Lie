using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.DataModels
{
    public class Timelineinfo
    {
        public int IdTimelineinfo { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public byte State { get; set; }
        public string City { get; set; }
        public string Context { get; set; }
        public byte Locked { get; set; }
        public int Owner { get; set; }
        public DateTime Updated { get; set; }
    }
}
