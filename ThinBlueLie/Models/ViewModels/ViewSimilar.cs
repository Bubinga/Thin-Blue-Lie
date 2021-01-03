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
        internal Timelineinfo Timelineinfo { get; set; }
        internal List<CommonPerson> Officers { get; set; }
        internal List<CommonPerson> Subjects { get; set; }
        internal ViewMedia Media { get; set; }
    }
}
