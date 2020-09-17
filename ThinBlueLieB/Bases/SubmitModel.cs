using DataAccessLibrary.DataModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ThinBlueLieB.Models
{
    public partial class SubmitBase
    {
        public class SubmitModel
        {
            [BindProperty]
            public ViewTimelineinfo Timelineinfos { get; set; }
            [BindProperty]           
            public List<ViewMedia> Medias { get; set; }
            [BindProperty]         
            public List<ViewOfficer> Officers { get; set; }
            [BindProperty]           
            public List<ViewSubject> Subjects { get; set; }
            public bool SignedIn { get; set; }            
            public int Misconducts { get; set; }
            public int Weapons { get; set; }
        }

    }
}
