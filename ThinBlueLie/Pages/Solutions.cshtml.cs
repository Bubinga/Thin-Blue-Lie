using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ThinBlueLie.Pages
{
    public class SolutionModel : PageModel
    {
        private readonly ILogger<SolutionModel> _logger;

        public SolutionModel(ILogger<SolutionModel> logger)
        {
            _logger = logger;
        }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
        public void OnGet()
        {
        }
    }
}
