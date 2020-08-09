using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ThinBlue;

namespace ThinBlueLie.Pages
{
    public class Test3Model
    {
        public List<Subjects> Subjects { get; set; }
        public Media Medias { get; set; }

    }
}