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
            Timelineinfos = new ViewTimelineinfo(),
            Medias = new List<ViewMedia> { new ViewMedia { ListIndex = 0 } },
            Officers = new List<ViewOfficer> { new ViewOfficer { ListIndex = 0 } },
            Subjects = new List<ViewSubject> { new ViewSubject { ListIndex = 0 } }
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
