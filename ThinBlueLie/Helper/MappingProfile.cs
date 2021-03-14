using AutoMapper;
using DataAccessLibrary.DataModels;
using System.Linq;
using ThinBlueLie.Models;
using static DataAccessLibrary.Enums.TimelineinfoEnums;
using static ThinBlueLie.Helper.Extensions.IntExtensions;
using static ThinBlueLie.Models.ViewModels.EditReviewModel;
using static ThinBlueLie.Searches.SearchClasses;

namespace ThinBlueLie.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects

            CreateMap<Officers, CommonPerson>()
               .ForMember(dest => dest.Race, opt => opt.MapFrom(src => (RaceEnum?)src.Race))
               .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => (SexEnum?)src.Sex));
            CreateMap<Subjects, CommonPerson>()
               .ForMember(dest => dest.Race, opt => opt.MapFrom(src => (RaceEnum?)src.Race))
               .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => (SexEnum?)src.Sex));
            CreateMap<Subjects, ViewSubject>()
                .ForMember(dest => dest.Race, opt => opt.MapFrom(src => (RaceEnum?)src.Race))
                .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => (SexEnum?)src.Sex));
            CreateMap<Officers, ViewOfficer>()
               .ForMember(dest => dest.Race, opt => opt.MapFrom(src => (RaceEnum?)src.Race))
               .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => (SexEnum?)src.Sex));

            CreateMap<ViewOfficer, Officers>();
            CreateMap<ViewSubject, Subjects>();
            CreateMap<ViewOfficer, DBOfficer>()
                .ForMember(dest => dest.Misconduct, opt => opt.MapFrom(src => src.Misconduct.Sum()))
                .ForMember(dest => dest.Weapon, opt => opt.MapFrom(src => src.Weapon.Sum()));
            CreateMap<ViewSubject, DBSubject>();
            CreateMap<ViewOfficer, CommonPerson>();
            CreateMap<ViewSubject, CommonPerson>();

            CreateMap<SimilarOfficer, SimilarPerson>();
            CreateMap<SimilarSubject, SimilarPerson>();
            CreateMap<SimilarOfficer, CommonPerson>();
            CreateMap<SimilarSubject, CommonPerson>();

            CreateMap<DBOfficer, CommonPerson>()
               .ForMember(dest => dest.Race, opt => opt.MapFrom(src => (RaceEnum?)src.Race))
               .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => (SexEnum?)src.Sex));
            CreateMap<DBSubject, CommonPerson>()
                .ForMember(dest => dest.Race, opt => opt.MapFrom(src => (RaceEnum?)src.Race))
               .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => (SexEnum?)src.Sex));
            CreateMap<DBOfficer, ViewOfficer>()
                .ForMember(dest => dest.Misconduct, opt => opt.MapFrom(src => IntToArray(src.Misconduct)))
                .ForMember(dest => dest.Weapon, opt => opt.MapFrom(src => IntToArray(src.Weapon)));
            CreateMap<DBSubject, ViewSubject>();

            CreateMap<ViewMedia, EditMedia>()
                .ForMember(dest => dest.Rank, opt => opt.MapFrom(src => (short)src.Rank))
                .ForMember(dest => dest.MediaType, opt => opt.MapFrom(src => (byte)src.MediaType))
                .ForMember(dest => dest.SourceFrom, opt => opt.MapFrom(src => (byte)src.SourceFrom));
            CreateMap<ViewMedia, Media>()
                .ForMember(dest => dest.Rank, opt => opt.MapFrom(src => (short)src.Rank))
                .ForMember(dest => dest.MediaType, opt => opt.MapFrom(src => (byte)src.MediaType))
                .ForMember(dest => dest.SourceFrom, opt => opt.MapFrom(src => (byte)src.SourceFrom));
            CreateMap<EditMedia, Media>();
            CreateMap<EditMedia, ViewMedia>();
            CreateMap<ViewTimelineinfo, Timelineinfo>()
                .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.SubmittedBy));
            CreateMap<Timelineinfo, ViewTimelineinfo>()
                .ForMember(dest => dest.SubmittedBy, opt => opt.MapFrom(src => src.Owner));

            //For Deep Cloning
            CreateMap<EditReviewSegment, EditReviewSegment>();
            CreateMap<Timelineinfo, Timelineinfo>();
            CreateMap<Media, Media>();
            CreateMap<DBOfficer, DBOfficer>();
            CreateMap<DBSubject, DBSubject>();
            CreateMap<Officers, Officers>();
            CreateMap<Subjects, Subjects>();

            CreateMap<SubmitModel, SubmitModel>();
            CreateMap<ViewTimelineinfo, ViewTimelineinfo>();
            CreateMap<ViewMedia, ViewMedia>();
            CreateMap<ViewOfficer, ViewOfficer>();
            CreateMap<ViewSubject, ViewSubject>();
        }
    }
}
