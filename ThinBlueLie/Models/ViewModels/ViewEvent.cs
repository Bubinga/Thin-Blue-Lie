using DataAccessLibrary.DataModels;
using System.Collections.Generic;
using ThinBlueLie.Models;

namespace ThinBlueLie.ViewModels
{
    public class ViewEvent
    {
        public Timelineinfo Data { get; set; }
        public List<ViewOfficer> Officers { get; set; }
        public List<ViewSubject> Subjects { get; set; }
        public List<ViewMedia> Medias { get; set; }
    }
}
