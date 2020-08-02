using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThinBlue;
using ThinBlueLie.Models;

namespace ThinBlueLie.Pages
{
    public class SubmitModel
    { 
        [BindProperty]
        public Timelineinfo Timelineinfo { get; set; }
        [BindProperty]
        public Media Medias { get; set; }
        public bool SignedIn { get; set; }

        public IList<string> SelectedWeapons { get; set; }
        public IList<SelectListItem> AvailableWeapons { get; set; }

        public IList<string> SelectedMisconducts { get; set; }
        public IList<SelectListItem> AvailableMisconducts { get; set; }

        public SubmitModel()
        {
            SelectedWeapons = new List<string>();
            AvailableWeapons = new List<SelectListItem>();

            SelectedMisconducts = new List<string>();
            AvailableMisconducts = new List<SelectListItem>();
        }

    }
}
