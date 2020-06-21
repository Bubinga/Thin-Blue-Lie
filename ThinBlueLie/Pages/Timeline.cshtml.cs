using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DataAccessLibrary.thinblue;
using MySql.Data.MySqlClient;
using DataAccessLibrary;


namespace ThinBlueLie.Pages
{
    public class TimelineModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Date { get; set; }       
        
        public void OnGet()
        {
            if (string.IsNullOrWhiteSpace(Date))
            {
                Date = DateTime.Now.ToString("d");
            }
        }
    }
}