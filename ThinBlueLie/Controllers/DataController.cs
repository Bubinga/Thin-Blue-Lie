using AutoMapper;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.DataModels;
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
    public class DataController : Controller
    {
        private readonly DataContext _datacontext;
        private readonly UserContext _usercontext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IMapper _mapper;

        public DataController(DataContext datacontext,
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
                //Set SubmittedBy to current logged in user
                model.Timelineinfos.SubmittedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _datacontext.Timelineinfo.Add(model.Timelineinfos); //############
                await _datacontext.SaveChangesAsync();
                var id = model.Timelineinfos.IdTimelineInfo;
                //Add the Armed property onto the data
                var subjectTemp = new List<ViewSubject>();
                foreach ((var subject, Int32 i) in model.Subjects.Select((value, i) => (value, i)))
                {
                    var asubject = (ViewSubject)subject;
                    asubject.Armed = model.Armed[i];
                    subjectTemp.Add(asubject);
                }
                foreach (var subject in subjectTemp)
                {
                    _datacontext.Subjects.Add(subject);         //############    //Combine these foreaches so that the prelimiary submits to get ids
                    await _datacontext.SaveChangesAsync();                        //all happen together and you don't have a bunch of SaveChangesAsync()
                    var TSlink = new TimelineinfoSubject()
                    {
                        IdTimelineinfo = id,
                        IdSubject = subject.IdSubject,
                        Armed = Convert.ToByte(subject.Armed)
                    };
                    _datacontext.TimelineinfoSubject.Add(TSlink); //############
                }
                foreach (var officer in model.Officers)
                {
                    _datacontext.Officers.Add(officer); //############
                    await _datacontext.SaveChangesAsync();
                    var TOlink = new TimelineinfoOfficer()
                    {
                        IdTimelineinfo = id,
                        IdOfficer = officer.IdOfficer,
                    };
                    _datacontext.TimelineinfoOfficer.Add(TOlink); //############
                }
                foreach (var media in model.Medias)
                {
                    media.SubmittedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    media.IdTimelineinfo = id;
                    _datacontext.Media.Add(media); //############
                }
                await _datacontext.SaveChangesAsync();

                Log((int)LogEnums.ActionEnum.Submit, model.Timelineinfos.IdTimelineInfo);
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
                model.Timelineinfos = await _datacontext.Timelineinfo.Where(i => i.IdTimelineInfo.Equals(id)).FirstOrDefaultAsync();
                model.Medias = await _datacontext.Media.Where(d => d.IdTimelineinfo.Equals(id)).ToListAsync();

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
            IList<Timelineinfo> SimilarEvents = _datacontext.Timelineinfo.Where(t => t.Date.Equals(TempDate)).ToList();
            ViewData["SimilarEvents"] = SimilarEvents;
            return PartialView("_SimilarPartial");
        }
        [Route("/Submit/GetOfficer")]
        public PartialViewResult GetOfficer(string? Name, int? State, int? Race, int? Sex)
        {
            //load events where date or officer/subject name is shared and load it into SimilarEvents.            
            var SimilarOfficers = _datacontext.Officers.Where(t => t.Name.Equals(Name)).ToList();
            ViewData["SimilarOfficers"] = SimilarOfficers;
            return PartialView("_SimilarPartial");
        }
        [Route("/Submit/GetSubject")]
        public PartialViewResult GetSubject(string? Name, int? State, int? Race, int? Sex)
        {
            //load events where date or officer/subject name is shared and load it into SimilarEvents.            
            var SimilarSubjects = _datacontext.Subjects.Where(t => t.Name.Equals(Name)).ToList();
            ViewData["SimilarSubjects"] = SimilarSubjects;
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
