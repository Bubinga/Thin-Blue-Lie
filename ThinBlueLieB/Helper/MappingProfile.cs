using AutoMapper;
using DataAccessLibrary.DataModels;
using Syncfusion.Blazor.Lists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLieB.Models;

namespace ThinBlueLieB.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<ViewOfficer, Officers>();
            CreateMap<ViewSubject, Subjects>();
            CreateMap<ViewMedia, Media>();
            CreateMap<ViewTimelineinfo, Timelineinfo>();
        }
    }
}
