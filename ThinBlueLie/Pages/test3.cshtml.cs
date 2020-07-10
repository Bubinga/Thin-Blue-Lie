using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ThinBlueLie.Pages
{
    public class Test3Model
    {
       [Required]
       [EmailAddress]
        public string Email { get; set; }

       [Required]
       [DataType(DataType.Password)]
       public string Password { get; set; }

       [Display(Name = "Remember me")]
       public bool RememberMe { get; set; }
        
    }
}