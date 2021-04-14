using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.DataModels
{
    public class EditSubject
    {
        public int IdEditHistory { get; set; }
        public int IdSubject { get; set; }
        public int EditCount { get; set; }
        public string Name { get; set; }
        public byte Race { get; set; }
        public byte Sex { get; set; }
        public DateTime DOB { get; set; }
        public byte Action { get; set; }
    }
}
