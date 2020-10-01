using DataAccessLibrary.DataModels;
using DataAccessLibrary.Enums;
using Microsoft.AspNetCore.Components;
using Syncfusion.Licensing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLieB.Helper;
using ThinBlueLieB.Helper.Extensions;
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

        internal IEnumerable<EnumExtensions.ListItem> States = EnumExtensions.GetDropdownList<TimelineinfoEnums.StateEnum>.Items;
        //internal IEnumerable<string> States = Enum.GetNames(typeof(TimelineinfoEnums.StateEnum));
        //internal IEnumerable<string> States = EnumHelper<TimelineinfoEnums.StateEnum>.GetNames(TimelineinfoEnums.StateEnum);
        //internal IEnumerable<string> States = TimelineinfoEnums.StateEnum.AmericanSamoa.GetAttributeOfType<DisplayAttribute>().Description;
        //add login suggestion

    }
}
