using DataAccessLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLie.Models
{
    public class ProfileModel
    {
        public List<ProfileInfo> Submissions { get; set; }
        public int AcceptedEdits { get; set; }
        public int Flags { get; set; }
        public int VotesCast { get; set; }
    }
    public class ProfileInfo : Timelineinfo
    {
        /// <summary>
        /// 0 = Unconfirmed 1 = Confirmed 2 = Rejected
        /// </summary>
        public int Status { get; set; }
        public bool Event { get; set; }
    }
}
