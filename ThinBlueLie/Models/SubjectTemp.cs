using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static ThinBlueLie.Models.TimelineinfoEnums;

namespace ThinBlue
{
    public class SubjectTemp :Subject
    {        
        public bool Armed { get; set; }
    }
}
