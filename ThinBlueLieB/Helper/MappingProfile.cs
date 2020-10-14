using AutoMapper;
using DataAccessLibrary.DataModels;
using DataAccessLibrary.Enums;
using Syncfusion.Blazor.Lists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLieB.Models;
using ThinBlueLieB.ViewModels;
using static ThinBlueLieB.Models.ViewSimilar;

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
            CreateMap<DisplayOfficer, ViewSimilarPerson>();
            CreateMap<DisplaySubject, ViewSimilarPerson>();
            CreateMap<BaseOfficer, DisplayOfficer>()
                .ForMember(dest => dest.Race, opt => opt.MapFrom(src => (TimelineinfoEnums.RaceEnum)src.Race))
                .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => (TimelineinfoEnums.SexEnum)src.Sex));
            CreateMap<BaseSubject, DisplaySubject>()
                 .ForMember(dest => dest.Race, opt => opt.MapFrom(src => (TimelineinfoEnums.RaceEnum)src.Race))
                .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => (TimelineinfoEnums.SexEnum)src.Sex)); 
        }
    }
}
