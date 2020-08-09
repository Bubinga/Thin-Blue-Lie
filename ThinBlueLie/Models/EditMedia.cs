using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThinBlue
{
    public partial class EditMedia : Media
    {
        public int IdEditMedia { get; set; }
        public int IdEdits { get; set; }
        [Required]
        //public byte MediaType { get; set; }
        //[Required]
        //public string SourceFile { get; set; }
        //[Required]
        //public byte Gore { get; set; }
        //[Required]
        //public byte SourceFrom { get; set; }
        //[Required]
        //[MaxLength(250)]
        //public string Blurb { get; set; }
        //public string SubmittedBy { get; set; }
        public int Confirmed { get; set; }

        public virtual Edits IdEditsNavigation { get; set; }
    }
}
