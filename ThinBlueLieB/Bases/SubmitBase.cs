using DataAccessLibrary.DataModels;
using DataAccessLibrary.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLieB.Helper;

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

        public class ListItem
        {
            public int Value { get; set; }
            public string Text { get; set; }
        }
        internal List<ListItem> States = Enum.GetValues(typeof(TimelineinfoEnums.StateEnum)).Cast<TimelineinfoEnums.StateEnum>().Select(
                         x => new ListItem { Text = EnumExtensions.GetEnumDisplayName((TimelineinfoEnums.StateEnum)x), Value = (int)x }).ToList();
        public DateTime Today { get; set; } = new DateTime(DateTime.Today.Year, DateTime.Now.Month, DateTime.Today.Day);
        public DateTime MinDate { get; set; } = new DateTime(1950, 1, 1);
        public DateTime? DateValue { get; set; } = null;
        public List<SimilarPerson> SimilarPeople { get; set; }
        //protected async Task SearchTest()
        //{
        //    SimilarPeople = await Searches.SearchOfficer("Adam Wright");
        //}
       

        protected Task HandleValidSubmit()
        {
            throw new NotImplementedException();
        }

        protected Task AddSubject()
        {
            throw new NotImplementedException();
        }
        protected Task AddOfficer()
        {
            throw new NotImplementedException();
        }
        //add login suggestion

    }
}
