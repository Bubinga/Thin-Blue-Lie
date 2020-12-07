namespace ThinBlueLie.Helper
{
    public class ConfigHelper
    {
        public static string GetConnectionString()
        {
            return Startup.ConnectionString; //returns DataDB connection string
        }
    }
}
