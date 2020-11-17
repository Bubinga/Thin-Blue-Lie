using Microsoft.AspNetCore.Http;
using Syncfusion.Blazor.Inputs;
using System.ComponentModel.DataAnnotations;
using Syncfusion.Blazor.Inputs.Internal;
using DataAccessLibrary.Enums;

namespace ThinBlueLieB.Models
{
    public class ViewMedia : DisplayMedia
    {
        public int IdMedia { get; set; }
        public int IdTimelineinfo { get; set; }
        public int? SubmittedBy { get; set; }

    }
}
