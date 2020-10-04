using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ThinBlueLieB.Helper.Algorithms.WebsiteProfiling
{
    public partial class WebsiteProfile
    {
        public string PrepareYoutubeData(string link)
        {          
            Uri uri = new Uri(link, UriKind.Absolute);
            var videoid = HttpUtility.ParseQueryString(uri.Query).Get("v");
            return videoid;
        }
    }
}
