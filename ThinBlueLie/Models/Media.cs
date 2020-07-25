using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlue
{
    public class Media
    {
        [Key]
        public int IdMedia { get; set; }
        public int MediaType { get; set; }
        public int IdTimelineInfo { get; set; }
        public int SourceFrom { get; set; }
        public int Gore { get; set; }
        public string SourceFile { get; set; }
        public string Blurb { get; set; }
        [Column(TypeName = "VARCHAR(50)")]
        public string SubmittedBy { get; set; }
    }
}
