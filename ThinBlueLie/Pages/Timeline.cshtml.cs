using System.Collections.Generic;
using DataAccessLibrary.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace ThinBlueLie.Pages
{
    public class TimelineModel
    {
        public TimelineModel()
        {
        }
        [BindProperty]
        public Flagged Flags { get; set; }

        public IList<TimelineinfoFull> Timelineinfos { get; set; }      
    }
}