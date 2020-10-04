using OpenGraphNet;
using OpenGraphNet.Namespaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLieB.Models;

namespace ThinBlueLieB.Helper.Algorithms.WebsiteProfiling
{
    public partial class WebsiteProfile
    {
        public static NewsArticle GetWebsiteData(string link)
        {
            OpenGraph graph = OpenGraph.ParseUrl(link);
            //NamespaceRegistry.Instance.AddNamespace(
            //    prefix: "article",
            //    schemaUri: "https://www.nytimes.com",
            //    requiredElements: new[] { "published"});
          
            NewsArticle showMedia = new NewsArticle();
            showMedia.Title = graph.Title;
            showMedia.Url = graph.Url;
            showMedia.Image = graph.Image;
            showMedia.Site = graph.Metadata["og:site_name"].First().Value;
            showMedia.Description = graph.Metadata["og:description"].First().Value;
            var normalTime = Convert.ToDateTime(graph.Metadata["article:published_time"].First().Value);
            if (normalTime != null) { showMedia.Date = normalTime; }
            else { showMedia.Date = Convert.ToDateTime(graph.Metadata["article:published"].First().Value); }

            return showMedia;
            //Title
            //Meta description
            //Image
            //Date?
            //Website Source i.e New York Times, probably just the logo
        }
    }
}
