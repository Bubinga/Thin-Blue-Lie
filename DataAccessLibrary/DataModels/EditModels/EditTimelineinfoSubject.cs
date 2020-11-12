using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.DataModels
{
    public class EditTimelineinfoSubject
    {
        public int IdEditsTimelineinfoSubject { get; set; }
        public int IdEditHistory { get; set; }
        public int IdTimelineinfo { get; set; }
        public int IdEditsSubject { get; set; }
        public byte Armed { get; set; }
        public int Age { get; set; }
    }
}
