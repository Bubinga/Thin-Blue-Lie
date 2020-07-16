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
        public int Type { get; set; }
        public int IdTimelineinfo { get; set; }
        public string SourceLocation { get; set; }
        public int Gore { get; set; }
        public int SourceFile { get; set; }
        public string Blurb { get; set; }
        [Column(TypeName = "VARCHAR(50)")]
        public string SubmittedBy { get; set; }
    }
}
