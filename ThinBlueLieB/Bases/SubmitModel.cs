using DataAccessLibrary.DataModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ThinBlueLieB.Helper.Validators;

namespace ThinBlueLieB.Models
{
    public partial class SubmitBase
    {
        public class SubmitModel
        {
            public const int MaximumOfficers = 15;
            public const int MaximumSubjects = 15;

            [BindProperty]
            [ValidateComplexType]
            public ViewTimelineinfo Timelineinfos { get; set; }
            [BindProperty]
            [ValidateComplexType]
            public List<ViewMedia> Medias { get; set; }
            [BindProperty]
            [ValidateComplexType]
            [PersonValidator(1, MaximumOfficers, "Officers")]
            public List<ViewOfficer> Officers { get; set; }
            [BindProperty]
            [ValidateComplexType]
            [PersonValidator(1, MaximumSubjects, "Subjects")]
            public List<ViewSubject> Subjects { get; set; }
        
        }

    }
}
