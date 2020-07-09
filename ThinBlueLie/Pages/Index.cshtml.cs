using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ThinBlue;
using ThinBlueLie.Models;

namespace ThinBlueLie.Pages
{
    public class IndexModel : PageModel
    {
        public readonly ILogger<IndexModel> _logger;
        public readonly ThinbluelieContext db;

        public IndexModel(ILogger<IndexModel> logger, ThinbluelieContext db)
        {
            _logger = logger;
            this.db = db;
        }

        public void OnGet()
        {
            var a = (TimelineinfoEnums.StatesEnum)43;
            Console.WriteLine(a);           
        }
    }
}
