using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;
using System.Web;


namespace ThinBlueLie.Pages
{    
    public class testModel : PageModel
    {
        public void OnGet()
        {
            BindData();
        }

        public void BindData()
        {
           
        }
    }
}