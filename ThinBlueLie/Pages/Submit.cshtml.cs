using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ThinBlue;

namespace ThinBlueLie.Pages
{
    public class SubmitModel : PageModel
    {
        private readonly ThinBlue.ThinbluelieContext _context;

        public SubmitModel(ThinBlue.ThinbluelieContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Timelineinfo Timelineinfo { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Timelineinfo.Add(Timelineinfo);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
