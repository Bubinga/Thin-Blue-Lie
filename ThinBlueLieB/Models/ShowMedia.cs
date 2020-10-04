using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLieB.Models
{
    public class NewsArticle
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Uri Image { get; set; }
        public DateTime Date { get; set; }
        public string Site { get; set; }
        public Uri Url { get; set; }

        //Title
        //Meta description
        //Image
        //Date?
        //Website Source i.e New York Times, probably just the logo
    }
}
