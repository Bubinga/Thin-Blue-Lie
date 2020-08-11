using System.Collections.Generic;
using DataAccessLibrary.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ThinBlueLie.Pages
{
    public class EditModel
    {
        [BindProperty]
        public Edits Timelineinfo { get; set; }
        [BindProperty]
        public List<Media> Medias { get; set; }
        [BindProperty]
        public List<Officers> Officers { get; set; }
        [BindProperty]
        public List<Subjects> Subjects { get; set; }

        public IList<string> SelectedWeapons { get; set; }
        public IList<SelectListItem> AvailableWeapons { get; set; }

        public IList<string> SelectedMisconducts { get; set; }
        public IList<SelectListItem> AvailableMisconducts { get; set; }

        public EditModel()
        {
            SelectedWeapons = new List<string>();
            AvailableWeapons = new List<SelectListItem>();

            SelectedMisconducts = new List<string>();
            AvailableMisconducts = new List<SelectListItem>();
        }        
    }
}