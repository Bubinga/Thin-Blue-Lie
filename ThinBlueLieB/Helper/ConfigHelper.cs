using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace ThinBlueLieB.Helper
{
    public class ConfigHelper
    {

        public static string GetUploadsDirectory()
        {
            return Startup.UploadsDirectory; //returns DataDB connection string
        }

        public static string GetConnectionString()
        {
            return Startup.ConnectionString; //returns DataDB connection string
        }
    }
}
