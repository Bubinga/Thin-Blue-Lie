using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pomelo.EntityFrameworkCore.MySql.Query.ExpressionVisitors.Internal;
using ThinBlue;
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

        [BindProperty(SupportsGet = true)]
        public string date { get; set; }
        //public IList<Timelineinfo> Timelineinfo { get; set; }
        //public Timelineinfo Timelineinfo { get; set; }
       
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

            return View("Pages/Timeline.cshtml");
        }
              
        public ActionResult GetFlagView()
        {
            return PartialView("Pages/Shared/_FlagPartial.cshtml");
        }

        [HttpPost]
        [Route("/Timeline")] //flagging
        public async Task<IActionResult> Flag(TimelineModel flagModel)
        {
            if (ModelState.IsValid)
            {
                if (!_signInManager.IsSignedIn(User))
                {
                    flagModel.Flags.IdUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
                }
                else 
                {
                    flagModel.Flags.IdUser = "null";
                }
                             
                _context.Flagged.Add(flagModel.Flags);
                await _context.SaveChangesAsync();

                return Ok(true);
            }
            return (IActionResult)flagModel;
        }

        [HttpGet]
        [Route("/Submit")]
        public IActionResult Submit()
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
        [Route("/Submit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(SubmitModel model)
        {           
            if (ModelState.IsValid)
            {
                //Add the checkboxes together and convert them to ints
                var weaponsSum = model.SelectedWeapons.Sum(x => Convert.ToInt32(x));
                var misconductsSum = model.SelectedMisconducts.Sum(x => Convert.ToInt32(x));
                //Put the sums from above into the Timeline model
                model.Timelineinfo.Weapon = weaponsSum;
                model.Timelineinfo.Misconduct = misconductsSum;

                _context.Timelineinfo.Add(model.Timelineinfo);                
                await _context.SaveChangesAsync();              

                return RedirectToAction("Success");
            }
            model.AvailableWeapons = GetWeapons();
            model.AvailableMisconducts = GetMisconducts();
            return View("Pages/Submit.cshtml", model);
        }

        [HttpPost]
        [Route("/Submit")]
        [ValidateAntiForgeryToken] //Add media form handler
        public async Task<IActionResult> MediaAdd(SubmitModel model)
        {
            if (ModelState.IsValid)
            {
                if (!_signInManager.IsSignedIn(User))
                {
                    //add them in Log appropriately
                }
                else 
                {
                    //add them in Log appropriately
                }
                model.Medias.IdTimelineinfo = model.Timelineinfo.IdTimelineInfo;
                model.Medias.SubmittedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Media.Add(model.Medias);
                await _context.SaveChangesAsync();
            }
            
            return View("Pages/Submit.cshtml", model);
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
