using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.DataModels
{
    public class EditMisconducts : Misconducts
    {
        public int IdEditHistory { get; set; }
        public new int? IdOfficer { get; set; }
        public new int? IdSubject { get; set; }
    }
}