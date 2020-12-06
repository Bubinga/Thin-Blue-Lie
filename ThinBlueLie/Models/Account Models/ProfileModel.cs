using DataAccessLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLieB.Models
{
    public class ProfileModel
    {
        public List<Timelineinfo> Submissions { get; set; }
        public int AcceptedEdits { get; set; }
        public int Flags { get; set; }
        public int VotesCast { get; set; }
    }
}
