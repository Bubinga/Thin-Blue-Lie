using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
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
            ViewBag.Count = 0;
            return View("Pages/Test3.cshtml");
        }

        [HttpPost]
        [Route("Test3")]
        public ActionResult Index(Test3Model model)
        {
            return View("Pages/Test3.cshtml", model);
        }
        
        [Route("Test3/MethodTest")]
        public ActionResult MethodTest()
        {
            return null;
        }

        public ActionResult Success()
        {          
            return View("./Index");
        }

    }
}
