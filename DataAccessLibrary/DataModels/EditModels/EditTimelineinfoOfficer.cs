﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.DataModels
{
    class EditTimelineinfoOfficer
    {
        public int IdEditsTimelineinfoOfficer { get; set; }
        public int IdEditHistory { get; set; }
        public int IdTimelineinfo { get; set; }
        public int IdEditsOfficer { get; set; }
        public int Misconduct { get; set; }
        public int Weapon { get; set; }
        public int Age { get; set; }
    }
}