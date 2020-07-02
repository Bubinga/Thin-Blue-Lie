using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ThinBlueLie.Pages
{
    public class RegisterModel : PageModel
    {
        //private readonly ThinBlue.ThinbluelieContext _context;

        //public RegisterModel(ThinBlue.ThinbluelieContext context)
        //{
        //    _context = context;
        //}

        public IActionResult OnGet()
        {
            return Page();
        }

        

        public RegisterModel()
        {
          
        }
                
       
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Username { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.



        //public async Task<IActionResult> OnPostAsync()
        //{
        //    var errors = ModelState.Values.SelectMany(v => v.Errors);
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }           

        //    //_context.Users.Add(Users);
        //     await _context.SaveChangesAsync();

        //     return RedirectToPage("./Index");        
         
        //}
    }
}

