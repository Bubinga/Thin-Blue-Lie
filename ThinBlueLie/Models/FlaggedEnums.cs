using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLie.Models
{
    public class FlaggedEnums
    {
        public enum FlagType { 
            [Display(Name = "Vulgar or Racist")]
            Vulgar,
            [Display(Name = "False Information")]
            False,
            [Display(Name = "Not Police Brutality")]
            NotBrutality,
            [Display(Name = "Duplicate Event")]
            Duplicate,
            Other
        }
    }
}
