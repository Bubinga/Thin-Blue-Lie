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
            [ValidateComplexType]
            public ViewTimelineinfo Timelineinfos { get; set; }
            [BindProperty]
            [ValidateComplexType]
            public List<ViewMedia> Medias { get; set; }
            [BindProperty]
            [ValidateComplexType]
            public List<ViewOfficer> Officers { get; set; }
            [BindProperty]
            [ValidateComplexType]
            public List<ViewSubject> Subjects { get; set; }
        
        }

    }
}
