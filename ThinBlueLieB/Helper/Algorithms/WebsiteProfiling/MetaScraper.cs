using HtmlAgilityPack;
using OpenGraphNet;
using OpenGraphNet.Namespaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ThinBlueLieB.Models;

namespace ThinBlueLieB.Helper.Algorithms.WebsiteProfiling
{
    public partial class MetaScraper
    {        
        /// <summary>
        /// Uses HtmlAgilityPack to get the meta information from a url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static MetaTags GetMetaData(string url)
        {
            // Get the URL specified
            var webGet = new HtmlWeb();
            var document = webGet.Load(url);
            var metaTags = document.DocumentNode.SelectNodes("//meta");
            MetaTags metaInfo = new MetaTags(url);
            if (metaTags != null)
            {
                int matchCount = 0;
                foreach (var tag in metaTags)
                {
                    var tagName = tag.Attributes["name"];
                    var tagContent = tag.Attributes["content"];
                    var tagProperty = tag.Attributes["property"];
                    string content = null;
                    if (tagContent != null)
                    {
                        content = HttpUtility.HtmlDecode(tagContent.Value);
                    }
                    if (tagName != null && tagContent != null)
                    {
                        switch (tagName.Value.ToLower())
                        {
                            case "title":
                                metaInfo.Title = content;
                                matchCount++;
                                break;
                            case "description":
                                metaInfo.Description = content;
                                matchCount++;
                                break;
                            case "site_name":
                                metaInfo.SiteName = content;
                                matchCount++;
                                break;
                            case "published_time":
                                metaInfo.Date = Convert.ToDateTime(content);
                                matchCount++;
                                break;
                            case "modified_time":
                                metaInfo.Date = Convert.ToDateTime(content);
                                matchCount++;
                                break;
                            case "article:published":
                                metaInfo.Date = Convert.ToDateTime(content);
                                matchCount++;
                                break;
                            case "article:modified":
                                metaInfo.Date = Convert.ToDateTime(content);
                                matchCount++;
                                break;
                            case "twitter:title":
                                metaInfo.Title = string.IsNullOrEmpty(metaInfo.Title) ? content : metaInfo.Title;
                                matchCount++;
                                break;
                            case "twitter:description":
                                metaInfo.Description = string.IsNullOrEmpty(metaInfo.Description) ? content : metaInfo.Description;
                                matchCount++;
                                break;
                            //case "keywords":
                            //    metaInfo.Keywords = tagContent.Value;
                            //    matchCount++;
                            //    break;
                            case "twitter:image":
                                metaInfo.Image = string.IsNullOrEmpty(metaInfo.Image) ? content : metaInfo.Image;
                                matchCount++;
                                break;
                        }
                    }
                    else if (tagProperty != null && tagContent != null)
                    {
                        switch (tagProperty.Value.ToLower())
                        {
                            case "og:title":
                                metaInfo.Title = string.IsNullOrEmpty(metaInfo.Title) ? content : metaInfo.Title;
                                matchCount++;
                                break;
                            case "og:description":
                                metaInfo.Description = string.IsNullOrEmpty(metaInfo.Description) ? content : metaInfo.Description;
                                matchCount++;
                                break;
                            case "og:image":
                                metaInfo.Image = string.IsNullOrEmpty(metaInfo.Image) ? content : metaInfo.Image;
                                matchCount++;
                                break;
                        }
                    }
                }
                //metaInfo.HasData = matchCount > 0;
            }
            return metaInfo;
        }
    }
}
