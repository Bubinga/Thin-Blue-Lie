using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.DataModels
{
    class EditOfficer
    {
        public int IdEditOfficer { get; set; }
        public int IdEditHistory { get; set; }
        public int IdOfficer { get; set; }
        public int EditCount { get; set; }
        public string Name { get; set; }
        public byte Race { get; set; }
        public byte Sex { get; set; }

    }
}
