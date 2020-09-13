using DataAccessLibrary.DataModels;
using DataAccessLibrary.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLieB.Helper;
using static ThinBlueLieB.Searches.SearchClasses;

namespace ThinBlueLieB.Models
{
    public partial class SubmitBase : ComponentBase
    {        
        public SubmitModel model = new SubmitModel()
        {
            Timelineinfos = new Timelineinfo(),
            Medias = new List<ViewMedia>(),
            Officers = new List<ViewOfficer>(),
            Subjects = new List<ViewSubject>()
        };

        public object[] Tools = new object[]{
        "Bold", "Italic", "Underline", "|",
        "Formats", "Alignments", "OrderedList", "UnorderedList",
        "Outdent", "Indent", "|", "CreateTable",
        "CreateLink", "|", "ClearFormat",
        "SourceCode", "|", "Undo", "Redo"
         };
       
        internal IReadOnlyList<Extensions.ListItem> States = Extensions.GetDropdownList<TimelineinfoEnums.StateEnum>.Items;        
           
       
        //add login suggestion

    }
}
