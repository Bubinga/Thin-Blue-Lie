using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.DataModels
{
    public class EditSubject
    {
        public int IdEditsSubject { get; set; }
        public int IdEditHistory { get; set; }
        public int IdSubject { get; set; }
        public int EditCount { get; set; }
        public string Name { get; set; }
        public byte Race { get; set; }
        public byte Sex { get; set; }
        public string Image { get; set; }
        public byte Local { get; set; }
        public byte Action { get; set; }
    }
}
