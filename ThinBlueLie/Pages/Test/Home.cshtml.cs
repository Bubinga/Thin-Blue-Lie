using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ThinBlueLie.Pages.Test
{
    public class HomeModel
    {
        public IList<string> SelectedFruits { get; set; }
        public IList<SelectListItem> AvailableFruits { get; set; }

        public HomeModel()
        {
            SelectedFruits = new List<string>();
            AvailableFruits = new List<SelectListItem>();
        }
    }
}
