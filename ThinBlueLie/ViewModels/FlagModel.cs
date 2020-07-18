using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ThinBlue;

namespace ThinBlueLie.ViewModels
{
    public class FlagModel
    {
        [BindProperty]
        public Flagged Flags { get; set; }
       
    }
}
