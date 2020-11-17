using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.DataModels
{
    class EditVotes
    {
        public int IdEditHistory { get; set; }
        public int UserId { get; set; }
        public short? Vote { get; set; }
    }
}
