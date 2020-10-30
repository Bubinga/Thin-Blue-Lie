using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.DataModels
{
    class EditTimelineinfoSubject
    {
        public int IdEditsTimelineinfoSubject { get; set; }
        public int IdEdits { get; set; }
        public int IdEditsSubject { get; set; }
        public byte Armed { get; set; }
        public int Age { get; set; }
    }
}
