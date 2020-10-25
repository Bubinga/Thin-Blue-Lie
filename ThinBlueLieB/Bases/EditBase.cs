using AutoMapper;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.DataModels;
using DataAccessLibrary.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLieB.Models;
using ThinBlueLieB.ViewModels;
using static ThinBlueLieB.Helper.ConfigHelper;
using static ThinBlueLieB.Models.SubmitBase;

namespace ThinBlueLieB.Bases
{
    public class EditBase : InformationBase
    {
        //public new SubmitModel model = new SubmitModel
        //{
        //    Timelineinfos = new ViewTimelineinfo(),
        //    Medias = new List<ViewMedia> { new ViewMedia { } },
        //    Officers = new List<ViewOfficer> { new ViewOfficer { } },
        //    Subjects = new List<ViewSubject> { new ViewSubject { } }
        //};
        internal uint Id;

        [Inject]
        public IMapper mapper { get; set; }
        //fill out timelineinfo
        //fill out media
        //fill out officers
        //fill out subjects
        internal async Task<SubmitModel> FetchDataAsync()
        {
            DataAccess data = new DataAccess();
            var query = "SELECT * From timelineinfo t where t.IdTimelineinfo = @id;";
            Timelineinfo timelineinfo = await data.LoadDataSingle<Timelineinfo, dynamic>(query, new { id = Id }, GetConnectionString());

            var mediaQuery = "SELECT m.MediaType, m.SourcePath, m.Gore, m.SourceFrom, m.Blurb, m.Credit, m.SubmittedBy, m.Rank From media m where m.IdTimelineinfo = @id Order By m.Rank;";
            var officerQuery = "SELECT o.Name, o.Race, o.Sex, t_o.Age, t_o.Misconduct, t_o.Weapon " +
                    "FROM timelineinfo t " +
                    "JOIN timelineinfo_officer t_o ON t.IdTimelineinfo = t_o.IdTimelineinfo " +
                    "JOIN officers o ON t_o.IdOfficer = o.IdOfficer " +
                    "WHERE t.IdTimelineinfo = @id ;";
            var subjectQuery = "SELECT s.Name, s.Race, s.Sex, t_s.Age, t_s.Armed " +
                    "FROM timelineinfo t " +
                    "JOIN timelineinfo_subject t_s ON t.IdTimelineinfo = t_s.IdTimelineinfo " +
                    "JOIN subjects s ON t_s.IdSubject = s.IdSubject " +
                    "WHERE t.IdTimelineinfo = @id;";

            //get media, officers, and subjects using timelineinfo id
            List<DisplayMedia> media = await data.LoadData<DisplayMedia, dynamic>(mediaQuery, new { id = Id }, GetConnectionString());
            List<DBOfficer> officers = await data.LoadData<DBOfficer, dynamic>(officerQuery, new { id = Id }, GetConnectionString());
            List<DBSubject> subjects = await data.LoadData<DBSubject, dynamic>(subjectQuery, new { id = Id }, GetConnectionString());
            var Info = new ViewTimelineinfo
            {
                Date = timelineinfo.Date.ToString("yyyy-MM-dd"),
                State = (TimelineinfoEnums.StateEnum?)timelineinfo.State,
                City = timelineinfo.City,
                Context = timelineinfo.Context,
                Locked = timelineinfo.Locked,
                SubmittedBy = timelineinfo.SubmittedBy,
                Verified = timelineinfo.Verified
            };
            var Media = mapper.Map<List<DisplayMedia>, List<ViewMedia>>(media);
            var Officers = mapper.Map<List<DBOfficer>, List<ViewOfficer>>(officers);
            var Subjects = mapper.Map<List<DBSubject>, List<ViewSubject>>(subjects);
            model = new SubmitModel
            {
                Timelineinfos = Info,
                Medias = Media,
                Officers = Officers,
                Subjects = Subjects
            };
            return model;

        }

        internal async void HandleValidSubmitAsync()
        {

        }
    }
}
