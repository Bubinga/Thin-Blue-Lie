using DataAccessLibrary.DataModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLieB.Models
{
    public class SubmitBase : ComponentBase
    {
        public class SubmitModel
        {
            [BindProperty]
            public Timelineinfo Timelineinfos { get; set; }
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
