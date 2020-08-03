using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pomelo.EntityFrameworkCore.MySql.Query.ExpressionVisitors.Internal;
using ThinBlue;
using ThinBlueLie.Models;
using ThinBlueLie.Pages;


namespace ThinBlueLie.Controllers
{
    public class TimelineController : Controller
    {

        private readonly ThinBlue.ThinbluelieContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public TimelineController(ThinBlue.ThinbluelieContext context,
                                 UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public void Log(int action, int IdTimeline)
        {
            var log = new Log { };
            log.TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // yyyy-mm-dd hh-mm-ss
            log.Action = action;
            log.IpAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            log.IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            log.IdTimelineinfo = IdTimeline;
            _context.Log.Add(log);
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
            model.Timelineinfos = await _context.Timelineinfo.Where(t => t.Date.Equals(date)).ToListAsync();
            //load data into ViewData to be used in the Timeline page
            ViewData["Timelineinfo"] = model.Timelineinfos;
            await GetTimeline(date, 0);
            return View("Pages/Timeline.cshtml");
        }
        public DateTime dateT { get; set; }
        public List<List<Timelineinfo>> dateData { get; set; }
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
                dateData[i] = _context.Timelineinfo.Where(t => t.Date.Equals(dates[i].ToString("yyyy-MM-dd"))).ToList();
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
                    flagModel.Flags.IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
                }
                else{
                    flagModel.Flags.IdUser = "null";
                }

                Log((int)LogEnums.ActionEnum.Flag, flagModel.Flags.IdTimelineInfo);
                _context.Flagged.Add(flagModel.Flags);
                await _context.SaveChangesAsync();

                return Ok(true);
            }
            return PartialView("Pages/Shared/_FlagPartial.cshtml",flagModel);
        }

        [HttpGet]
        [Route("/Submit")]
        public ActionResult Submit()
        {
            var model = new SubmitModel
            {
                AvailableWeapons = GetWeapons(),
                AvailableMisconducts = GetMisconducts()
            };
            GetSimilar(DateTime.Today.ToString("yyyy-MM-dd"));
            return View("Pages/Submit.cshtml", model);
        }

        [HttpPost]
        [Route("/Submit/Submit")]
        [ValidateAntiForgeryToken] //Main form handler
        public async Task<IActionResult> Submit(SubmitModel model)
        {           
            if (ModelState.IsValid)
            {
                //Add the checkboxes together and convert them to ints to put into the model
                var weaponSum = model.SelectedWeapons.Sum(x => Convert.ToInt32(x));
                var misconductSum = model.SelectedMisconducts.Sum(x => Convert.ToInt32(x));
                //Put the sums from above into the Timeline model
                model.Timelineinfo.Weapon = weaponSum;
                model.Timelineinfo.Misconduct = misconductSum;
                model.Timelineinfo.SubmittedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Timelineinfo.Add(model.Timelineinfo);                
                await _context.SaveChangesAsync();

                Log((int)LogEnums.ActionEnum.Submit, model.Timelineinfo.IdTimelineInfo);
                return RedirectToAction("Success");
            }
            model.AvailableWeapons = GetWeapons();
            model.AvailableMisconducts = GetMisconducts();
            return PartialView("Pages/Submit.cshtml", model);
        }

      
        [Route("/Submit/MediaAdd")]
        [ValidateAntiForgeryToken] //Add media form handler
        public async Task<IActionResult> MediaAdd(SubmitModel model)
        {
            if (ModelState.IsValid)
            {
              //  model.Medias.IdTimelineInfo = model.Timelineinfo.IdTimelineInfo;
                model.Medias.SubmittedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Media.Add(model.Medias);
                await _context.SaveChangesAsync();
               // var x = 10;
                //Log((int)LogEnums.ActionEnum.Submit, model.Timelineinfo.IdTimelineInfo);

            }
            
            return View("Pages/Submit.cshtml", model);
        }

        [BindProperty(SupportsGet = true)]
        public string idString { get; set; }

        [HttpGet]
        [Route("/Edit")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Edit()
        {
            var model = new EditModel
            {
                AvailableWeapons = GetWeapons(),
                AvailableMisconducts = GetMisconducts()
            };

            idString = Request.Query["id"];
            int id = Int32.Parse(idString);
            //if (date == null | string.IsNullOrWhiteSpace(Request.Query["d"]))
            //{
            //    return NotFound();
            //}

            //query database using query string
            // model.Timelineinfos = await _context.Timelineinfo.FromSqlRaw($"SELECT * FROM `thin-blue-lie`.timelineinfo WHERE Date = '{date}'").ToListAsync();
            model.Timelineinfo = await _context.Timelineinfo.Where(i => i.IdTimelineInfo.Equals(id)).FirstOrDefaultAsync();
            model.Medias = await _context.Media.Where(d => d.IdTimelineInfo.Equals(id)).ToListAsync();

            //load data into ViewData to be used in the Edit page
            ViewData["Timelineinfo"] = model.Timelineinfo;
            ViewData["Media"] = model.Medias;

            return View("Pages/Edit.cshtml", model);
        }

        [HttpPost]
        [Route("/Edit")]
        public async Task<IActionResult> Edit(Edit model)
        {
            model.Verified = 1;
            return Ok(true);
        }

            [Route("/Submit/CheckSignedIn")]
        public ActionResult CheckSignedIn()
        {
            return Ok(_signInManager.IsSignedIn(User));
        }      

        [Route("/Submit/MoreMedia")]
        public ActionResult GetMediaPartial()
        {
            return PartialView("_MediaPartial");
        }

        public ActionResult Success()
        {
            return View("Pages/Index.cshtml");
            //display thank you banner
        }

#nullable enable
        public string TempDate { get; set; }
#nullable enable
        public string OName { get; set; }
#nullable enable
        public string Sname { get; set; }

        [Route("/Submit/GetSimilar")]        
        public PartialViewResult GetSimilar(string? TempDate)
        {
            //load events where date or officer/subject name is shared and load it into SimilarEvents.            
            IList<Timelineinfo> SimilarEvents = _context.Timelineinfo.Where(t => t.Date.Equals(TempDate)).ToList();
            ViewData["SimilarEvents"] = SimilarEvents;
            return PartialView("_SimilarPartial");
        }


        private IList<SelectListItem> GetWeapons()
        {
            return new List<SelectListItem>
        {
            new SelectListItem {Text = "Body", Value = "1"},
            new SelectListItem {Text = "Projectile", Value = "2"},
            new SelectListItem {Text = "Taser", Value = "4"},
            new SelectListItem {Text = "Tear Gas", Value = "8"},
            new SelectListItem {Text = "Vehicle", Value = "16"},
            new SelectListItem {Text = "Gun", Value = "32"},
        };
        }
        private IList<SelectListItem> GetMisconducts()
        {
            return new List<SelectListItem>
        {
            new SelectListItem {Text = "Unnecessary use of Force", Value = "1"},
            new SelectListItem {Text = "Killing of Pets", Value = "2"},
            new SelectListItem {Text = "Evidence Planting/Falsification/Spoilation", Value = "4"},
            new SelectListItem {Text = "Non-Violent Harassment/Intimidation", Value = "8"},
            new SelectListItem {Text = "Negligence", Value = "16"},
            new SelectListItem {Text = "Unwarranted Suizure of Property/Theft", Value = "32"},
            new SelectListItem {Text = "Unwarranted Search", Value = "64"},
            new SelectListItem {Text = "False Arrest/Detention", Value = "128"},
            new SelectListItem {Text = "Abuse of Authority for Personal Gain", Value = "256"},
        };
        }
    }
}
