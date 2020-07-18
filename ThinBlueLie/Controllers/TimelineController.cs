﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pomelo.EntityFrameworkCore.MySql.Query.ExpressionVisitors.Internal;
using ThinBlue;
using ThinBlueLie.Pages;
using ThinBlueLie.ViewModels;

namespace ThinBlueLie.Controllers
{
    public class TimelineController : Controller
    {

        private readonly ThinBlue.ThinbluelieContext _context;

        public TimelineController(ThinBlue.ThinbluelieContext context)
        {
            _context = context;
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
        [Route("/Timeline")]
        public async Task<IActionResult> Timeline(FlagModel flagModel)
        {
            if (ModelState.IsValid)
            {
                flagModel.Flags.IdTimelineInfo = 1;
                flagModel.Flags.IdUser = "testIdUser";              

                _context.Flagged.Add(flagModel.Flags);
                await _context.SaveChangesAsync();
            }
            return PartialView("Pages/Shared/_FlagPartial.cshtml", flagModel);
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
