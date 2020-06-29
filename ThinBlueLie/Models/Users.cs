using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLibrary.thinblue
{
    public partial class Users
    {
        public int IdUsers { get; set; }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        
        public string Password { get; set; }
    }
}
