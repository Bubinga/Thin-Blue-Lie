using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ThinBlue;
using ThinBlueLie.Pages;

namespace ThinBlueLie.Controllers
{
    public class TimelineController : Controller
    {

        private readonly ThinBlue.ThinbluelieContext _context;

        public TimelineController(ThinBlue.ThinbluelieContext context)
        {
            _context = context;
        }


        

        [HttpGet]
        public IActionResult Submit()
        {
            return View("Pages/Submit.cshtml");
        }
        [HttpPost]
        [Route("/Submit")]
        public async Task<IActionResult> OnPostAsync(SubmitModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Pages/Submit.cshtml");
            }

            _context.Timelineinfo.Add(model.Timelineinfo);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
            
        }
    }
}
