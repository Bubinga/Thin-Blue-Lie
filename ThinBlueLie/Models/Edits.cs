using System;
using System.Collections.Generic;

namespace ThinBlue
{
    public partial class Edits : Timelineinfo
    {
        public Edits()
        {
            EditMedia = new HashSet<EditMedia>();
        }

        public int IdEdits { get; set; }
        //public string Date { get; set; }
        //public byte State { get; set; }
        //public string City { get; set; }
        //public byte Misconduct { get; set; }
        //public byte? Weapon { get; set; }
        //public string Context { get; set; }
        //public byte Locked { get; set; }
        //public string SubmittedBy { get; set; }
        public byte Confirmed { get; set; }
        //public int IdTimelineInfo { get; set; }

        public virtual Timelineinfo IdTimelineInfoNavigation { get; set; }
        public virtual new Aspnetusers SubmittedByNavigation { get; set; }
        public virtual ICollection<EditMedia> EditMedia { get; set; }
    }
}
