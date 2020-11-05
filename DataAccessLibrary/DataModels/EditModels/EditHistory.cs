using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.DataModels.EditModels
{
    class EditHistory
    {
        public int IdEditHistory { get; set; }
        public DateTime Timestamp { get; set; }
        public byte Confirmed { get; set; }
        public string SubmittedBy { get; set; }
        public byte IdTimelineinfo { get; set; }
        public byte Edits { get; set; }
        public byte EditMedia { get; set; }
        public byte Officers { get; set; }
        public byte Subjects { get; set; }
        public byte Timelineinfo_Subject { get; set; }
        public byte Timelineinfo_Officer { get; set; }
    }
}
