using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLieB.Models;
using static ThinBlueLieB.Models.SubmitBase;

namespace ThinBlueLieB.Bases
{
    public class EditBase : InformationBase
    {
        public new SubmitModel model = new SubmitModel()
        {
            Timelineinfos = new ViewTimelineinfo(),
            Medias = new List<ViewMedia> { new ViewMedia {} },
            Officers = new List<ViewOfficer> { new ViewOfficer { } },
            Subjects = new List<ViewSubject> { new ViewSubject { } }
        };
        protected override Task OnInitializedAsync()
        {
            
        }
        //fill out timelineinfo
        //fill out media
        //fill out officers
        //fill out subjects

    }
}
