using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ThinBlue;
using ThinBlueLie.Models;

namespace ThinBlueLie.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ThinBlue.ThinbluelieContext _context;

        public LoginModel(ThinBlue.ThinbluelieContext context)
        {
            _context = context;
        }

        public Users Users { get; set; }
        [Required]
        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Users = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);

            if (Users == null)
            {
              return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            _context.Users.Add(Users);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
