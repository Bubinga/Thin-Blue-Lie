using DataAccessLibrary.DataModels;
using System.Collections.Generic;
using ThinBlueLieB.Models;

namespace ThinBlueLieB.ViewModels
{
    public class ViewEvent
    {
        public Timelineinfo Data { get; set; }
        public List<ViewOfficer> Officers { get; set; }
        public List<ViewSubject> Subjects { get; set; }
        public List<DisplayMedia> Medias { get; set; }
    }
}
