using DataAccessLibrary.DataModels;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLie.ViewModels
{
    public class ViewSimilar
    {
        public Timelineinfo Timelineinfo { get; set; }
        public List<string> Officers { get; set; }
        public List<string> Subjects { get; set; }
        public Media Media { get; set; }
    }
}
