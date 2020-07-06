using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ThinBlueLie.Pages.Test
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("Test/Home")]
        public ActionResult Index()
        {
            var model = new HomeModel
            {
                AvailableFruits = GetFruits()
            };
            return View("Pages/Test/Home.cshtml", model);
        }

        [HttpPost]
        [Route("Test/Home")]
        public ActionResult Index(HomeModel model)
        {
            if (ModelState.IsValid)
            {
                var fruits = string.Join(",", model.SelectedFruits);

                // Save data to database, and redirect to Success page.

                return RedirectToAction("Success");
            }
            model.AvailableFruits = GetFruits();
            return View("Pages/Test/Home.cshtml", model);
        }

        public ActionResult Success()
        {
            return View();
        }

        private IList<SelectListItem> GetFruits()
        {
            return new List<SelectListItem>
        {
            new SelectListItem {Text = "Apple", Value = "Apple"},
            new SelectListItem {Text = "Pear", Value = "Pear"},
            new SelectListItem {Text = "Banana", Value = "Banana"},
            new SelectListItem {Text = "Orange", Value = "Orange"},
        };
        }
    }
}
