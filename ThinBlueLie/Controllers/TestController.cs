using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ThinBlueLie.Pages;

namespace ThinBlueLie.Controllers
{
    public class TestController : Controller
    {
        [HttpGet]
        [Route("/Test3")]
        public ActionResult Test3()
        {
            var model = new Test3Model
            {
                AvailableWeapons = GetWeapons()
            };
            return View("Pages/Test3.cshtml", model);
        }

        [HttpPost]
        [Route("/Test3")]
        public ActionResult Index(Test3Model model)
        {
            if (ModelState.IsValid)
            {
                var fruits = string.Join(",", model.SelectedWeapons);

                // Save data to database, and redirect to Success page.

                return RedirectToAction("Success");
            }
            model.AvailableWeapons = GetWeapons();
            return View("Pages/Test3.cshtml", model);
        }

        public ActionResult Success()
        {          
            return View("./Index");
        }

        private IList<SelectListItem> GetWeapons()
        {
            return new List<SelectListItem>
        {
            new SelectListItem {Text = "Body", Value = "0"},
            new SelectListItem {Text = "Projectile", Value = "2"},
            new SelectListItem {Text = "Taser", Value = "4"},
            new SelectListItem {Text = "Tear Gas", Value = "8"},
            new SelectListItem {Text = "Vehicle", Value = "16"},
            new SelectListItem {Text = "Gun", Value = "32"},
        };
        }

    }
}
