using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ThinBlue;


namespace ThinBlueLie.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ThinBlue.ThinbluelieContext _context;

        public LoginModel(ThinBlue.ThinbluelieContext context)
        {
            _context = context;
        }

        [Required]
        [BindProperty]
        public string Email { get; set; }
        [Required]
        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {            
             
            await _context.SaveChangesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


             // _context.Users.Add(Users);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
