using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ThinBlue;
using MySql.Data.MySqlClient;
using DataAccessLibrary;
using Microsoft.EntityFrameworkCore;

namespace ThinBlueLie.Pages
{
    public class TimelineModel : PageModel
    {
        private readonly ThinBlue.ThinbluelieContext _context;

        public TimelineModel(ThinBlue.ThinbluelieContext context)
        {
            _context = context;
        }
        public IList<Timelineinfo> Timelineinfo { get; set; }
        public string date { get; set; }

        public async Task OnGet()
        {
            if (string.IsNullOrWhiteSpace(Request.Query["d"]))
            {
                DateTime today = DateTime.Today;
                date = today.ToString("yyyy/MM/dd");
                Response.Redirect("/Timeline?d=" + (date));

            }
            Timelineinfo = await _context.Timelineinfo.ToListAsync();
        }
    }
}