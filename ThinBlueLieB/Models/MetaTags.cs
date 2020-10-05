using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLieB.Models
{
    public class MetaTags
    {
        public bool HasData { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Image { get; set; }
        public DateTime? Date { get; set; }
        public string? SiteName { get; set; }
        public string Url { get; set; }

        public MetaTags(string url)
        {
            Url = url;
            HasData = false;
        }

        public MetaTags(string url, string title, string description, string keywords, string imageUrl, string siteName)
        {
            Url = url;
            Title = title;
            Description = description;
            //Keywords = keywords;
            Image = imageUrl;
            SiteName = siteName;
        }
        //Title
        //Meta description
        //Image
        //Date?
        //Website Source i.e New York Times, probably just the logo
    }
}
