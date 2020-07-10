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
        [Route("Test3")]
        public ActionResult Test3()
        {
            return View("Pages/Test3.cshtml");
        }

        [HttpPost]
        [Route("Test3")]
        public ActionResult Index(Test3Model model)
        {
            return View("Pages/Test3.cshtml", model);
        }

        public ActionResult Success()
        {          
            return View("./Index");
        }

    }
}
