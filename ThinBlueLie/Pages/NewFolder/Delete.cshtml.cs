using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccessLibrary.thinblue;

namespace ThinBlueLie.Pages.NewFolder
{
    public class DeleteModel : PageModel
    {
        private readonly DataAccessLibrary.thinblue.ThinbluelieContext _context;

        public DeleteModel(DataAccessLibrary.thinblue.ThinbluelieContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Timelineinfo Timelineinfo { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Timelineinfo = await _context.Timelineinfo.FirstOrDefaultAsync(m => m.IdTimelineInfo == id);

            if (Timelineinfo == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Timelineinfo = await _context.Timelineinfo.FindAsync(id);

            if (Timelineinfo != null)
            {
                _context.Timelineinfo.Remove(Timelineinfo);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
