using DataAccessLibrary.DataModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ThinBlueLie.Helper.Validators;

namespace ThinBlueLie.Models
{
    public class SubmitModel
    {
        public const int MaximumOfficers = 15;
        public const int MaximumSubjects = 15;
        public const int MaximumMedia = 20;

        [BindProperty]
        [ValidateComplexType]
        public ViewTimelineinfo Timelineinfos { get; set; }
        [BindProperty]
        [ValidateComplexType]
        [ListCountValidator(1, MaximumMedia, "Media")]
        public List<ViewMedia> Medias { get; set; }
        [BindProperty]
        [ValidateComplexType]
        [ListCountValidator(1, MaximumOfficers, "Officers")]
        public List<ViewOfficer> Officers { get; set; }
        [BindProperty]
        [ValidateComplexType]
        [ListCountValidator(1, MaximumSubjects, "Subjects")]
        public List<ViewSubject> Subjects { get; set; }
        [BindProperty]
        [ValidateComplexType]
        public List<ViewMisconduct> Misconducts { get; set; }        
    }
}
