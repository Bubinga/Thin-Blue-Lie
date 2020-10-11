using DataAccessLibrary.DataModels;
using DataAccessLibrary.Enums;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.RichTextEditor;
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

        //public object[] Tools = new object[]{
        //"Bold", "Italic", "Underline", "|",
        //"Formats", "Alignments", "OrderedList", "UnorderedList",
        //"Outdent", "Indent", "|", "CreateTable",
        //"CreateLink", "|", "ClearFormat",
        //"SourceCode", "|", "Undo", "Redo"
        // };

        public List<ToolbarItemModel> Tools = new List<ToolbarItemModel>()
        {
            new ToolbarItemModel() { Command = ToolbarCommand.Bold },
            new ToolbarItemModel() { Command = ToolbarCommand.Italic },
            new ToolbarItemModel() { Command = ToolbarCommand.Underline },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.Formats },
            new ToolbarItemModel() { Command = ToolbarCommand.Alignments },
            new ToolbarItemModel() { Command = ToolbarCommand.OrderedList },
            new ToolbarItemModel() { Command = ToolbarCommand.UnorderedList },
            new ToolbarItemModel() { Command = ToolbarCommand.Outdent },
            new ToolbarItemModel() { Command = ToolbarCommand.Indent },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.CreateLink },
            new ToolbarItemModel() { Command = ToolbarCommand.CreateTable },
         // new ToolbarItemModel() { Command = ToolbarCommand.Image }, TODO add support for context inline images
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.ClearFormat },
            new ToolbarItemModel() { Command = ToolbarCommand.SourceCode },
            new ToolbarItemModel() { Command = ToolbarCommand.Separator },
            new ToolbarItemModel() { Command = ToolbarCommand.Undo },
            new ToolbarItemModel() { Command = ToolbarCommand.Redo }
         };

        public IEnumerable<string> States = EnumExtensions.GetEnumDisplayNames<TimelineinfoEnums.StateEnum>();

        //add login suggestion

    }
}
