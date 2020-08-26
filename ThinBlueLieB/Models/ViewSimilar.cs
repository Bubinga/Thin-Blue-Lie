using DataAccessLibrary.DataModels;
using Microsoft.VisualBasic;
using Syncfusion.Blazor.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLieB.Models
{
    public class ViewSimilar
    {       
        internal class ViewSimilarPerson
        {
            public string Name { get; set; }
            public byte Race { get; set; }
            public byte Age { get; set; }
            public byte Sex { get; set; }
        }
        internal Timelineinfo Timelineinfo { get; set; }
        internal List<ViewSimilarPerson> Officers { get; set; }
        internal List<ViewSimilarPerson> Subjects { get; set; }
        internal Media Media { get; set; }
    }
}
