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
using static ThinBlueLieB.Helper.Extensions.IntExtensions;
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
            CreateMap<DisplayMedia, ViewMedia>();
            CreateMap<ViewTimelineinfo, Timelineinfo>();
            CreateMap<Timelineinfo, ViewTimelineinfo>();
            CreateMap<DBOfficer, ViewSimilarPerson>();
            CreateMap<DBSubject, ViewSimilarPerson>();
            CreateMap<ViewOfficer, ViewSimilarPerson>();
            CreateMap<ViewSubject, ViewSimilarPerson>();
            CreateMap<DBOfficer, ViewOfficer>()
                .ForMember(dest => dest.Misconduct, opt => opt.MapFrom(src => IntToArray(src.Misconduct)))
                .ForMember(dest => dest.Weapon, opt => opt.MapFrom(src => IntToArray(src.Weapon)));
            CreateMap<DBSubject, ViewSubject>();
            CreateMap<ViewMedia, EditMedia>();
        }
    }
}
