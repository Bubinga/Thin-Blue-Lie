using DataAccessLibrary.DataModels;
using DataAccessLibrary.Enums;
using Syncfusion.Blazor.Gantt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ThinBlueLie.Helper.Algorithms.WebsiteProfiling;
using ThinBlueLie.Models;
using static DataAccessLibrary.Enums.MediaEnums;

namespace ThinBlueLie.Helper.Algorithms
{
    public class LinkConverter
    {
        public static async Task<string> GetLinkFromDataEditAsync(Media media, bool IsSource = false)
        {
            var result = GetLinkFromData(media, IsSource);
            if (result == "Error")
            {
                //var news = new MetaTags(media.SourcePath); //initialized with HasData = false
                if (IsSource)
                {
                    return media.SourcePath;
                }
                else
                {
                    var news = await MetaScraper.GetMetaData(media.SourcePath);
                    return news.Image;
                }
            }
            else
            {
                return result;
            }
        }

        public static string GetLinkFromData(Media media, bool video = false, bool procressed = true)
        {
            if ((MediaTypeEnum)media.MediaType == MediaTypeEnum.Video)
            {
                if ((SourceFromEnum)media.SourceFrom == SourceFromEnum.Youtube)
                {
                    if (video)
                    {
                        var path = $"https://www.youtube-nocookie.com/embed/{media.SourcePath}?rel=0&enablejsapi=1&iv_load_policy=3&version=3&modestbranding=1";
                        return path;
                    }
                    else
                    {
                        if (procressed == false)
                        {
                            Uri uri = new Uri(media.SourcePath, UriKind.Absolute);
                            media.SourcePath = HttpUtility.ParseQueryString(uri.Query).Get("v");                           
                        }
                        var path = $"https://i.ytimg.com/vi/{media.SourcePath}/0.jpg";
                        return path;

                    }
                }
                //Add support for reddit
                // image: https://i.redd.it/0u3pdpo3zgv51.jpg
                //Add support for other sourcefroms
            }
            if ((MediaTypeEnum)media.MediaType == MediaTypeEnum.Image)
            {
                if ((SourceFromEnum)media.SourceFrom == SourceFromEnum.Device)
                {
                    var path = ConfigHelper.GetUploadsDirectory() + media.SourcePath + ".jpg";
                    return path;
                }
                else if ((SourceFromEnum)media.SourceFrom == SourceFromEnum.Reddit)
                {
                    var path = $"https://i.redd.it/{media.SourcePath}.jpg";
                    return path;
                }
                else
                {
                    return media.SourcePath;
                }
            }
            return "Error";
        }
    }
}
