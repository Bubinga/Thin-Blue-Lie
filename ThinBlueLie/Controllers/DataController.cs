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
using System.Diagnostics.Tracing;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Threading.Tasks;
using ThinBlueLie.Pages;
using ThinBlueLie.ViewModels;

namespace ThinBlueLie.Controllers
{
    public class DataController : Controller
    {
        private readonly UserContext _usercontext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMapper _mapper;

        public DataController(UserContext usercontext,
                                 UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager,
                                 IMapper mapper)
        {
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
            log.IdTimelineinfo = IdTimeline;
            //_datacontext.Log.Add(log);
            //  return Ok(true);
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
            ViewBag.MediaCount = 0;
            ViewBag.SubjectCount = 0;
            ViewBag.OfficerCount = 0;
            GetSimilar(DateTime.Today.ToString("yyyy-MM-dd"));
            return View("Pages/Submit.cshtml", model);
        }

        [HttpPost]
        [Route("/Submit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(SubmitModel model)
        {
            if (ModelState.IsValid)
            {
                //Add the checkboxes together and convert them to ints to put into the model
                if (model.SelectedWeapons.Count != 0)
                {
                    model.Timelineinfos.Weapon = (byte?)model.SelectedWeapons.Sum(x => Convert.ToInt32(x));
                }
                model.Timelineinfos.Misconduct = (byte)model.SelectedMisconducts.Sum(x => Convert.ToInt32(x));

                //Fill out SubmittedBy and add to Database
                var user = model.Timelineinfos.SubmittedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _datacontext.Timelineinfo.Add(model.Timelineinfos); //Add Timelineinfo to data database
                await _datacontext.SaveChangesAsync();
                var id = model.Timelineinfos.IdTimelineinfo;

                //Add Subjects to subjects table and junction table

                foreach (var subject in model.Subjects)
                {
                    var armed = subject.Armed;
                    var SameAs = subject.SameAs;
                    var Subject = _mapper.Map<Subjects>(subject);
                    _datacontext.Subjects.Add(Subject);         //############    //Combine these foreaches so that the prelimiary submits to get ids
                    await _datacontext.SaveChangesAsync();                        //all happen together and you don't have a bunch of SaveChangesAsync()
                    var TSlink = new TimelineinfoSubject()
                    {
                        IdTimelineinfo = id,
                        IdSubject = subject.IdSubject,
                        Armed = Convert.ToByte(SameAs)
                    };
                    _datacontext.TimelineinfoSubject.Add(TSlink); //############
                }

                //Add Officers to officers table and junction table
               
                foreach (var officer in model.Officers)
                {
                    if (officer.SameAs == false)
                    {
                        var Officer = _mapper.Map<Officers>(officer);
                        _datacontext.Officers.Add(Officer);
                    }                  
                    await _datacontext.SaveChangesAsync();
                    var TOlink = new TimelineinfoOfficer()
                    {
                        IdTimelineinfo = id,
                        IdOfficer = officer.IdOfficer,
                    };
                    _datacontext.TimelineinfoOfficer.Add(TOlink); //############
                }

                var medias = _mapper.Map<List<Media>>(model.Medias);
                //Add Media to Media Table
                foreach (var media in medias)
                {
                    media.SubmittedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    media.IdTimelineinfo = id;
                    _datacontext.Media.Add(media); //############
                }
                await _datacontext.SaveChangesAsync();

                Log((int)LogEnums.ActionEnum.Submit, model.Timelineinfos.IdTimelineinfo);
                return null;
            }
            model.AvailableWeapons = GetWeapons();
            model.AvailableMisconducts = GetMisconducts();
            if (model.Timelineinfos.Date == null)
            {
                GetSimilar(DateTime.Today.ToString("yyyy-MM-dd"));
            }
            else
            {
                GetSimilar(model.Timelineinfos.Date);
            }
            return View("Pages/Submit.cshtml", model);
        }


        [BindProperty(SupportsGet = true)]
        public string idString { get; set; }
        [BindProperty(SupportsGet = true)]
        public string date { get; set; }
        [HttpGet]
        [Route("/Edit")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Edit()
        {
            if ((User != null) && User.Identity.IsAuthenticated)
            {
                idString = Request.Query["id"];
                if (date == null | string.IsNullOrWhiteSpace(Request.Query["d"]))
                {
                    return NotFound();
                }
                var model = new SubmitModel
                {
                    AvailableWeapons = GetWeapons(),
                    AvailableMisconducts = GetMisconducts()
                };
                int id = Int32.Parse(idString);
                model.Timelineinfos = await _datacontext.Timelineinfo.Where(i => i.IdTimelineinfo.Equals(id)).FirstOrDefaultAsync();
                model.Medias = _mapper.Map<List<ViewMedia>>(await _datacontext.Media.Where(d => d.IdTimelineinfo.Equals(id)).ToListAsync());

                //load data into ViewData to be used in the Edit page
                ViewData["EditTimelineinfo"] = model.Timelineinfos;
                ViewData["EditMedia"] = model.Medias;
                ViewBag.MediaCount = 0;
                ViewBag.SubjectCount = 0;
                ViewBag.OfficerCount = 0;
                GetSimilar(model.Timelineinfos.Date);

                return View("Pages/Edit.cshtml", model);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("/Edit")]
        public async Task<IActionResult> Edit(SubmitModel model)
        {
            if (ModelState.IsValid)
            {
                //Add the checkboxes together and convert them to ints to put into the model
                if (model.SelectedWeapons.Count != 0)
                {
                    model.Timelineinfos.Weapon = (byte?)model.SelectedWeapons.Sum(x => Convert.ToInt32(x));
                }
                model.Timelineinfos.Misconduct = (byte)model.SelectedMisconducts.Sum(x => Convert.ToInt32(x));
                //Set SubmittedBy to current logged in user
                model.Timelineinfos.SubmittedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //var emodel = (Edits)model.Timelineinfos; //Convert the Timelineinfo data into the Edit class which has an additional Confirmed property.
                //emodel.Confirmed = 0;
                //_context.Edits.Add(emodel); //add to Edit Table
                //await _context.SaveChangesAsync();
                //var id = emodel.IdTimelineInfo; //To be put into media
                //foreach (var media in model.Medias)
                //{
                //    var editMedia = (EditMedia)media; //Convert the Timelineinfo data into the Edit class which has an additional Confirmed property.
                //    editMedia.Confirmed = 0;
                //    editMedia.SubmittedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //   // editMedia.IdTimelineInfo = id;
                //    _context.EditMedia.Add(editMedia); //Add to edit Media table
                //}
                //await _context.SaveChangesAsync();

                //Log((int)LogEnums.ActionEnum.Submit, emodel.IdTimelineInfo);
                return null;
            }
            model.AvailableWeapons = GetWeapons();
            model.AvailableMisconducts = GetMisconducts();
            if (model.Timelineinfos.Date == null)
            {
                GetSimilar(DateTime.Today.ToString("yyyy-MM-dd"));
            }
            else
            {
                GetSimilar(model.Timelineinfos.Date);
            }
            return View("Pages/Submit.cshtml", model);
        }

        [Route("/Submit/CheckSignedIn")]
        public ActionResult CheckSignedIn()
        {
            return Ok(_signInManager.IsSignedIn(User));
        }

        [Route("/Submit/MoreMedia")]
        public ActionResult GetMediaPartial(int data)
        {
            ViewBag.MediaCount = data;
            return PartialView("_MediaPartial");
        }
        [Route("/Submit/MoreSubjects")]
        public ActionResult GetSubjectPartial(int data)
        {
            ViewBag.SubjectCount = data;
            return PartialView("_SubjectPartial");
        }
        [Route("/Submit/MoreOfficers")]
        public ActionResult GetOfficerPartial(int data)
        {
            ViewBag.OfficerCount = data;
            return PartialView("_OfficerPartial");
        }


        public ActionResult Success()
        {
            return View("Pages/Index.cshtml");
            //display thank you banner
        }


        [Route("/Submit/GetSimilar")]
        public PartialViewResult GetSimilar(string? TempDate)
        {
            //load events where date or officer/subject name is shared and load it into SimilarEvents.            
            List<object> SimilarEvents = new List<object>();
            var SimilarTimelineinfos = _datacontext.Timelineinfo.FromSqlRaw("SELECT t.Date, t.IdTimelineinfo, t.Context, t.State, t.City, t.Verified, t.SubmittedBy From timelineinfo t where t.date = {0};", TempDate).ToList();
            foreach ((var Event, Int32 i) in SimilarTimelineinfos.Select((Event, i) => (Event, i)))
            {
                var id = Event.IdTimelineinfo;
                var Officers = _datacontext.Officers.FromSqlRaw("SELECT o.Name, o.Race, o.Sex, o.IdOfficer FROM timelineinfo JOIN timelineinfo_officer ON timelineinfo.IdTimelineinfo = timelineinfo_officer.IdTimelineinfo JOIN officers o ON timelineinfo_officer.IdOfficer = o.IdOfficer WHERE timelineinfo.IdTimelineinfo = {0}",id).ToList();
                var Subjects = _datacontext.Subjects.FromSqlRaw("SELECT o.Name, o.Race, o.Sex, o.IdSubject FROM timelineinfo JOIN timelineinfo_subject ON timelineinfo.IdTimelineinfo = timelineinfo_subject.IdTimelineinfo JOIN subjects o ON timelineinfo_subject.IdSubject = o.IdSubject WHERE timelineinfo.IdTimelineinfo = {0}", id).ToList();
                var Media = _datacontext.Media.FromSqlRaw("Select * From media m Where(m.IdTimelineinfo = {0}); ",id).ToList();
                //add timelineinfos, officers, subject, and media to list at end
                var ListEvent = new List<object>();
                SimilarEvents.AddRange(new List<object>() {Event, Officers, Subjects, Media });
                //SimilarEvents.Add(Event);
                //SimilarEvents.Add(Officers);
                //SimilarEvents.Add(Subjects);
                //SimilarEvents.Add(Media);
            }
            //IList<Timelineinfo> SimilarEvents = _datacontext.Timelineinfo.FromSqlRaw("GetStudents").ToList();
            ViewData["SimilarEvents"] = SimilarEvents;
            return PartialView("_SimilarPartial");
        }
        [Route("/Submit/GetOfficer")]
        public PartialViewResult GetOfficer(string? Name)
        {
            //load events where date or officer/subject name is shared and load it into SimilarEvents.            
            var SimilarOfficers = _datacontext.Officers.Where(t => t.Name.Equals(Name)).ToList();
            ViewData["SimilarPeople"] = SimilarOfficers;
            return PartialView("_SimilarPersonPartial");
        }
        [Route("/Submit/GetSubject")]
        public PartialViewResult GetSubject(string? Name)
        {
            //load events where date or officer/subject name is shared and load it into SimilarEvents.            
            var SimilarSubjects = _datacontext.Subjects.Where(t => t.Name.Equals(Name)).ToList();
            ViewData["SimilarPeople"] = SimilarSubjects;
            return PartialView("_SimilarPersonPartial");
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
