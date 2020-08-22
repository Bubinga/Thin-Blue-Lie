using DataAccessLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLie.ViewModels
{
    public class ViewEvent
    {
        public Timelineinfo Data { get; set; }
        public List<Officers> Officers { get; set; }
        public List<Subjects> Subjects { get; set; }
        public List<Media> Medias { get; set; }
    }
}
