using System;
using System.Collections.Generic;

namespace ThinBlue
{
    public partial class EditMedia : Media
    {
        public int IdEditMedia { get; set; }
        public int IdEdits { get; set; }
        //public byte MediaType { get; set; }
        //public string SourceFile { get; set; }
        //public byte Gore { get; set; }
        //public byte SourceFrom { get; set; }
        //public string Blurb { get; set; }
        //public string SubmittedBy { get; set; }
        public byte Confirmed { get; set; }

        public virtual Edits IdEditsNavigation { get; set; }
        public virtual new Aspnetusers SubmittedByNavigation { get; set; }
    }
}
