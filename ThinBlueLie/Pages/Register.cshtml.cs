using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DataAccessLibrary.thinblue;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ThinBlueLie.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly DataAccessLibrary.thinblue.ThinbluelieContext _context;

        public RegisterModel(DataAccessLibrary.thinblue.ThinbluelieContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        
        public Users Users { get; set; }        

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [BindProperty]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.



        public async Task<IActionResult> OnPostAsync()
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
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

//byte[] salt = new byte[128 / 8];
//using (var rng = RandomNumberGenerator.Create())
//{
//    rng.GetBytes(salt);
//}
//Convert.ToBase64String(salt);
//string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
//password: ConfirmPassword,
//salt: salt,
//prf: KeyDerivationPrf.HMACSHA1,
//iterationCount: 10000,
//numBytesRequested: 256 / 8));
//Users.Password = hashed;