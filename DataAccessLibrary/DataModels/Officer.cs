using DataAccessLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLibrary.DataModels
{
    public class Officer : CommonPerson
    {
        public int IdOfficer { get; set; }
        public string? Image { get; set; }
        public byte? Local { get; set; }
    }
}
