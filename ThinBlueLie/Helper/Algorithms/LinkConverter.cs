using DataAccessLibrary.DataModels;
using System;
using System.Threading.Tasks;
using System.Web;
using ThinBlueLie.Helper.Algorithms.WebsiteProfiling;
using ThinBlueLie.Models;
using static DataAccessLibrary.Enums.MediaEnums;

namespace ThinBlueLie.Helper.Algorithms
{
    public class LinkConverter
    {
        //public static async Task<ViewMedia> GetLinkFromDataEditAsync(ViewMedia media, bool IsSource = false)
        //{
        //    await media.GetData(media);
        //    if (media.Processed == false)
        //    {
        //        //var news = new MetaTags(media.SourcePath); //initialized with HasData = false
        //        if (IsSource)
        //        {
        //            return media.SourcePath;
        //        }
        //        else
        //        {
        //            var news = await MetaScraper.GetMetaData(media.SourcePath);
        //            media.ContentUrl = media.Thumbnail = news.Image;
        //        }
        //    }

        //}

        //public static async Task<ViewMedia> GetLinkFromData(ViewMedia media, bool video = false, bool procressed = true)
        //{
        //    if ((MediaTypeEnum)media.MediaType == MediaTypeEnum.Video)
        //    {
        //        if ((SourceFromEnum)media.SourceFrom == SourceFromEnum.Youtube)
        //        {
        //            if (video)
        //            {
        //                var path = $"https://www.youtube-nocookie.com/embed/{media.SourcePath}?rel=0&enablejsapi=1&iv_load_policy=3&version=3&modestbranding=1";
        //                media.ContentUrl = path;
        //                return media;
        //            }
        //            else
        //            {
        //                if (procressed == false)
        //                {
        //                    Uri uri = new Uri(media.SourcePath, UriKind.Absolute);
        //                    media.SourcePath = HttpUtility.ParseQueryString(uri.Query).Get("v");                           
        //                }
        //                var path = $"https://i.ytimg.com/vi/{media.SourcePath}/0.jpg";
        //                return thumbnail ?? path;

        //            }
        //        }
        //        if ((SourceFromEnum)media.SourceFrom == SourceFromEnum.Reddit)
        //        {
        //            if (procressed)
        //            {
        //                if (video) // -> https://v.redd.it/4ymh7g5fzfv51
        //                {
        //                    media.ContentUrl = $"https://v.redd.it/{media.SourcePath}DASH_720.mp4";
        //                    return media;
        //                }
        //                else
        //                {
        //                    return media;
        //                }
        //            }
        //            else
        //            {
        //                var newMedia = await WebsiteProfile.GetRedditDataAsync(new ViewMedia()
        //                {
        //                    SourceFrom = (SourceFromEnum?)media.SourceFrom,
        //                    SourcePath = media.SourcePath
        //                });
        //                return await GetLinkFromData(newMedia, video, true);                       
                        
        //            }
        //        }
        //        //Add support for reddit
        //        // image: https://i.redd.it/0u3pdpo3zgv51.jpg
        //        //Add support for other sourcefroms
        //    }
        //    if ((MediaTypeEnum)media.MediaType == MediaTypeEnum.Image)
        //    {
        //        if ((SourceFromEnum)media.SourceFrom == SourceFromEnum.Device)
        //        {
        //            var path = "/Uploads/" + media.SourcePath + ".jpg";
        //            return path;
        //        }
        //        else if ((SourceFromEnum)media.SourceFrom == SourceFromEnum.Reddit)
        //        {
        //            var path = $"https://i.redd.it/{media.SourcePath}.jpg";
        //            return path;
        //        }
        //        else
        //        {
        //            return media.SourcePath;
        //        }
        //    }
        //    media.SourcePath = media.ContentUrl = "Error";
        //    return media;
        //}
    }
}
