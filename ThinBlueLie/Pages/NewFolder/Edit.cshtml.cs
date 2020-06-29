using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThinBlue;

namespace ThinBlueLie.Pages.NewFolder
{
    public class EditModel : PageModel
    {
        private readonly ThinBlue.ThinbluelieContext _context;

        public EditModel(ThinBlue.ThinbluelieContext context)
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

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Timelineinfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimelineinfoExists(Timelineinfo.IdTimelineInfo))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TimelineinfoExists(int id)
        {
            return _context.Timelineinfo.Any(e => e.IdTimelineInfo == id);
        }
    }
}
