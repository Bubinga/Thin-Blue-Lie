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

            CreateMap<Officer, CommonPerson>()
               .ForMember(dest => dest.Race, opt => opt.MapFrom(src => (RaceEnum?)src.Race))
               .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => (SexEnum?)src.Sex));
            CreateMap<Subject, CommonPerson>()
               .ForMember(dest => dest.Race, opt => opt.MapFrom(src => (RaceEnum?)src.Race))
               .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => (SexEnum?)src.Sex));
            CreateMap<Subject, ViewSubject>()
                .ForMember(dest => dest.Race, opt => opt.MapFrom(src => (RaceEnum?)src.Race))
                .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => (SexEnum?)src.Sex));
            CreateMap<Officer, ViewOfficer>()
               .ForMember(dest => dest.Race, opt => opt.MapFrom(src => (RaceEnum?)src.Race))
               .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => (SexEnum?)src.Sex));

            CreateMap<ViewOfficer, Officer>();
            CreateMap<ViewSubject, Subject>();
            CreateMap<ViewOfficer, Officer>();
                //.ForMember(dest => dest.Misconduct, opt => opt.MapFrom(src => src.Misconduct.Sum()))
                //.ForMember(dest => dest.Weapon, opt => opt.MapFrom(src => src.Weapon.Sum()));
            CreateMap<ViewSubject, Subject>();
            CreateMap<ViewOfficer, CommonPerson>();
            CreateMap<ViewSubject, CommonPerson>();

            CreateMap<SimilarOfficer, SimilarPerson>();
            CreateMap<SimilarSubject, SimilarPerson>();
            CreateMap<SimilarOfficer, CommonPerson>();
            CreateMap<SimilarSubject, CommonPerson>();

            CreateMap<Officer, CommonPerson>()
               .ForMember(dest => dest.Race, opt => opt.MapFrom(src => (RaceEnum?)src.Race))
               .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => (SexEnum?)src.Sex));
            CreateMap<Subject, CommonPerson>()
                .ForMember(dest => dest.Race, opt => opt.MapFrom(src => (RaceEnum?)src.Race))
               .ForMember(dest => dest.Sex, opt => opt.MapFrom(src => (SexEnum?)src.Sex));
            CreateMap<Officer, ViewOfficer>();
               // .ForMember(dest => dest.Misconduct, opt => opt.MapFrom(src => IntToArray(src.Misconduct)))
               // .ForMember(dest => dest.Weapon, opt => opt.MapFrom(src => IntToArray(src.Weapon)));
            CreateMap<Subject, ViewSubject>();

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


            CreateMap<EditMisconducts, ViewMisconduct>()
                .ForMember(dest => dest.Misconduct, opt => opt.MapFrom(src => IntToArray(src.Misconduct)))
                .ForMember(dest => dest.Weapon, opt => opt.MapFrom(src => IntToArray(src.Weapon)))
                .ForMember(dest => dest.Officer, opt => opt.MapFrom(src => new ViewOfficer() {IdOfficer = src.IdOfficer}))
                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => new ViewSubject() {IdSubject = src.IdSubject}));
            CreateMap<ViewMisconduct, EditMisconducts>()
                .ForMember(dest => dest.Misconduct, opt => opt.MapFrom(src => src.Misconduct.Sum()))
                .ForMember(dest => dest.Weapon, opt => opt.MapFrom(src => src.Weapon.Sum()))
                .ForMember(dest => dest.IdOfficer, opt => opt.MapFrom(src => src.Officer.IdOfficer))
                .ForMember(dest => dest.IdSubject, opt => opt.MapFrom(src => src.Subject.IdSubject));
            CreateMap<Misconducts, ViewMisconduct>()
                .ForMember(dest => dest.Misconduct, opt => opt.MapFrom(src => IntToArray(src.Misconduct)))
                .ForMember(dest => dest.Weapon, opt => opt.MapFrom(src => IntToArray(src.Weapon)));
            CreateMap<ViewMisconduct, Misconducts>()
                .ForMember(dest => dest.Misconduct, opt => opt.MapFrom(src => src.Misconduct.Sum()))
                .ForMember(dest => dest.Weapon, opt => opt.MapFrom(src => src.Weapon.Sum()));

            //For Deep Cloning
            CreateMap<EditReviewSegment, EditReviewSegment>();
            CreateMap<Timelineinfo, Timelineinfo>();
            CreateMap<Media, Media>();
            CreateMap<Officer, Officer>();
            CreateMap<Subject, Subject>();
            CreateMap<Officer, Officer>();
            CreateMap<Subject, Subject>();

            CreateMap<SubmitModel, SubmitModel>();
            CreateMap<ViewTimelineinfo, ViewTimelineinfo>();
            CreateMap<ViewMedia, ViewMedia>();
            CreateMap<ViewOfficer, ViewOfficer>();
            CreateMap<ViewSubject, ViewSubject>();
        }
    }
}
