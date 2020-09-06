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
        public DateTime Today { get; set; } = new DateTime(DateTime.Today.Year, DateTime.Now.Month, DateTime.Today.Day);
        public DateTime MinDate { get; set; } = new DateTime(1950, 1, 1);
        public DateTime? DateValue { get; set; } = new DateTime(DateTime.Today.Year, DateTime.Now.Month, DateTime.Today.Day);
        public List<SimilarPerson>? SimilarPeople { get; set; }
        public List<ViewSimilar>? SimilarEvents { get; set; }


        protected Task HandleValidSubmit()
        {
            throw new NotImplementedException();
        }       
       
        //add login suggestion

    }
}
