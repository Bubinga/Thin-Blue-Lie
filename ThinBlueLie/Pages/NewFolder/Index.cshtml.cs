using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ThinBlue;

namespace ThinBlueLie.Pages.NewFolder
{
    public class IndexModel : PageModel
    {
        private readonly ThinBlue.ThinbluelieContext _context;

        public IndexModel(ThinBlue.ThinbluelieContext context)
        {
            _context = context;
        }

        public IList<Timelineinfo> Timelineinfo { get;set; }

        public async Task OnGetAsync()
        {
            Timelineinfo = await _context.Timelineinfo.ToListAsync();
        }
    }
}
