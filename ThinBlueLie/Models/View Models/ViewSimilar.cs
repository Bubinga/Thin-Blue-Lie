using DataAccessLibrary.DataModels;
using Microsoft.VisualBasic;
using Syncfusion.Blazor.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLibrary.Enums;

namespace ThinBlueLie.Models
{
    public class ViewSimilar
    {       
        internal class ViewSimilarPerson
        {
            public string? Name { get; set; }
            public TimelineinfoEnums.RaceEnum? Race { get; set; }
            public TimelineinfoEnums.SexEnum? Sex { get; set; }
            public byte? Age { get; set; }          
        }
        internal Timelineinfo Timelineinfo { get; set; }
        internal List<ViewSimilarPerson> Officers { get; set; }
        internal List<ViewSimilarPerson> Subjects { get; set; }
        internal Media Media { get; set; }
    }
}
