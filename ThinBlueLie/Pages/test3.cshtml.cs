using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ThinBlueLie.Pages
{
    public class Test3Model
    {
        public IList<string> SelectedWeapons { get; set; }
        public IList<SelectListItem> AvailableWeapons { get; set; }

        public Test3Model()
        {
            SelectedWeapons = new List<string>();
            AvailableWeapons = new List<SelectListItem>();
        }
    }
}