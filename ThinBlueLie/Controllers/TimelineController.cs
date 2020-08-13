using AutoMapper;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.DataModels;
using DataAccessLibrary.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ThinBlueLie.Pages;
using ThinBlueLie.ViewModels;

namespace ThinBlueLie.Controllers
{
    public class TimelineController : Controller
    {

        private readonly DataContext _datacontext;
        private readonly UserContext _usercontext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMapper _mapper;

        public TimelineController(DataContext datacontext,
                                 UserContext usercontext,
                                 UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager,
                                 IMapper mapper)
        {
            _datacontext = datacontext;
            _usercontext = usercontext;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public void Log(uint action, int IdTimeline)
        {
            var log = new Log { };
            log.TimeStamp = DateTime.Now; // yyyy-mm-dd hh-mm-ss
            log.Action = action;
            log.IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            log.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            log.IdTimelineInfo = IdTimeline;
            _datacontext.Log.Add(log);
            //  return Ok(true);
        }


        [BindProperty(SupportsGet = true)]
        public string date { get; set; }
              
        [HttpGet]
        [Route("/Timeline")]
        public async Task<IActionResult> Timeline(TimelineModel model)
        {
            date = Request.Query["d"];
            //If query string is null fill out with current date
            if (date == null | string.IsNullOrWhiteSpace(Request.Query["d"]))
            {
                var today = DateTime.Today.ToString("yyyy-MM-dd");
                Response.Redirect("/Timeline?d=" + (today));
                date = today;
            }
            //query database using query string
           // model.Timelineinfos = await _context.Timelineinfo.FromSqlRaw($"SELECT * FROM `thin-blue-lie`.timelineinfo WHERE Date = '{date}'").ToListAsync();
            model.Timelineinfos = await _datacontext.Timelineinfo.Where(t => t.Date.Equals(date)).ToListAsync();
            var events = new List<object>();
            foreach (var Event in model.Timelineinfos)
            {
                using (var context = new DataContext())
                {
                    
                }
                //Get list of subject/officer ids where Idtimeline is one in the date
                var SubjectIds = await _datacontext.TimelineinfoSubject.Where(id1 => id1.IdTimelineinfo.Equals(Event.IdTimelineInfo)).Select(s => s.IdSubject).ToListAsync();
                var OfficerIds = await _datacontext.TimelineinfoOfficer.Where(id2 => id2.IdTimelineinfo.Equals(Event.IdTimelineInfo)).Select(o => o.IdOfficer).ToListAsync();
                var Subjects = await _datacontext.Subjects.Where(s1 => s1.IdSubject.Equals(SubjectIds)).ToListAsync();
                var Officers = await _datacontext.Officers.Where(o1 => o1.IdOfficer.Equals(OfficerIds)).ToListAsync();
                var Medias = await _datacontext.Media.Where(m => m.IdTimelineinfo.Equals(Event.IdTimelineInfo)).ToListAsync();
                events.Add(Subjects);
                events.Add(Officers);
                events.Add(Medias);
                //var Events = await _context.Timelineinfo.Include(x => x.Subjects).Include(x => x.Officers).Where(t => t.Date.Equals(date)).ToListAsync();
            }
            //load data into ViewData to be used in the Timeline page
            ViewData["Timelineinfo"] = model.Timelineinfos;
            await GetTimeline(date, 0);
            return View("Pages/Timeline.cshtml");
        }

        [Route("Timeline/GetTimeline")]  
        public async Task<IActionResult> GetTimeline(string? current, int? dateChange)
        {
            //string[] weekDays = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            if (current != null && dateChange != null)
            {
                date = Convert.ToDateTime(current).AddDays((double)dateChange).ToString("yyyy-MM-dd");
            }
            else if (date == null | string.IsNullOrWhiteSpace(Request.Query["d"]))
            {
                date = DateTime.Today.ToString("yyyy-MM-dd");               
            }            
            else
            {
                date = Request.Query["d"];
            }
            DateTime dateT = Convert.ToDateTime(date); //convert date from string to DateTime
            DateTime[] dates = new DateTime[7];
            var weekStart = dateT.AddDays(-(int)dateT.DayOfWeek); //week start date
            if (dateChange == null) {
                dates[0] = dateT.AddDays(-(int)dateT.DayOfWeek);
            }
            else {
                dates[0] = dateT.AddDays(-(int)dateT.DayOfWeek);
            }
            for (int i = 1; i < 7; i++) {
                dates[i] = weekStart.AddDays(i);
            }          
            ViewData["Dates"] = dates;
            var dateData = new List<List<Timelineinfo>>(new List<Timelineinfo>[7]);           
            for (int i = 0; i < 7; i++)
            {                       
                dateData[i] = await _datacontext.Timelineinfo.Where(t => t.Date.Equals(dates[i].ToString("yyyy-MM-dd"))).ToListAsync();
            }                
            ViewData["DateData"] = dateData;
            return PartialView("_TimelinePartial");
        }
        
        [HttpPost]
        [Route("/Timeline")] //flagging
        public async Task<IActionResult> Flag(TimelineModel flagModel)
        {
            if (ModelState.IsValid)
            {
                if (!_signInManager.IsSignedIn(User)){
                    flagModel.Flags.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                }
                else{
                    flagModel.Flags.UserId = "null";
                }

                Log((int)LogEnums.ActionEnum.Flag, flagModel.Flags.IdTimelineInfo);
                _datacontext.Flagged.Add(flagModel.Flags);
                await _datacontext.SaveChangesAsync();

                return Ok(true);
            }
            return PartialView("Pages/Shared/_FlagPartial.cshtml",flagModel);
        }    
        
    }
}
