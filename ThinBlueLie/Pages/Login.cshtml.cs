using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DataAccessLibrary.thinblue;

namespace ThinBlueLie.Pages
{
    public class LoginModel : PageModel
    {
        private readonly DataAccessLibrary.thinblue.ThinbluelieContext _context;

        public LoginModel(DataAccessLibrary.thinblue.ThinbluelieContext context)
        {
            _context = context;
        }

        public Users Users { get; set; }       

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            Users = await _context.Users.FirstOrDefaultAsync(m => m.IdUsers == id);

            //if (Users == null)
            //{
            //    return NotFound();
            //}
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
