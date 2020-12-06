using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace ThinBlueLie.Helper
{
    public class ConfigHelper
    {

        public static string GetUploadsDirectory()
        {
            return "/Uploads"; //returns DataDB connection string
        }

        public static string GetConnectionString()
        {
            return Startup.ConnectionString; //returns DataDB connection string
        }
    }
}
