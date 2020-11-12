using AutoMapper;
using DataAccessLibrary.DataModels;
using DataAccessLibrary.Enums;
using Syncfusion.Blazor.Lists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLieB.Models;
using ThinBlueLieB.Models.View_Models;
using ThinBlueLieB.ViewModels;
using static DataAccessLibrary.Enums.TimelineinfoEnums;
using static ThinBlueLieB.Helper.Extensions.IntExtensions;
using static ThinBlueLieB.Models.View_Models.EditReviewModel;
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
            CreateMap<Subjects, ViewSubject>()
                .ForMember(dest => dest.Race, opt => opt.MapFrom(src => (RaceEnum?)src.Race))
                .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => (SexEnum?)src.Sex));
            CreateMap<Officers, ViewOfficer>()
               .ForMember(dest => dest.Race, opt => opt.MapFrom(src => (RaceEnum?)src.Race))
               .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => (SexEnum?)src.Sex));
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
            CreateMap<EditMedia, Media>();
            CreateMap<Officers, CommonPerson>();
            CreateMap<Subjects, CommonPerson>();
            CreateMap<EditReviewSegment, EditReviewSegment>();
            CreateMap<Timelineinfo, Timelineinfo>();
            CreateMap<Media, Media>();
            CreateMap<DBOfficer, DBOfficer>();
            CreateMap<DBSubject, DBSubject>();
            CreateMap<Officers, Officers>();
            CreateMap<Subjects, Subjects>();
        }
    }
}
