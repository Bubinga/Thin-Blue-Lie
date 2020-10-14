using DataAccessLibrary.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using ThinBlueLie.ViewModels;

namespace ThinBlueLie.Pages
{
    public class SubmitModel
    { 
        [BindProperty]
        public TimelineinfoFull Timelineinfos { get; set; }
        [BindProperty]
        public List<ViewMedia> Medias { get; set; }
        [BindProperty]
        public List<ViewOfficer> Officers { get; set; }
        [BindProperty]
        public List<ViewSubject> Subjects { get; set; }     
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
