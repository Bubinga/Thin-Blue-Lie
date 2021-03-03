using DataAccessLibrary.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using ThinBlueLie.Models;
using static DataAccessLibrary.Enums.MediaEnums;

namespace ThinBlueLie.Helper.Algorithms.WebsiteProfiling
{
    public static partial class WebsiteProfile
    {
        /// <summary>
        /// Gets url of source for reddit links
        /// </summary>
        public static async Task<ViewMedia> GetRedditDataAsync(ViewMedia media)
        {
            var url = media.OriginalUrl;
            var source = media.SourceFrom;
            MediaTypeEnum mediaType = (MediaTypeEnum)media.MediaType;
            url = url.Remove(url.Length - 1, 1).Insert(url.Length - 1, ".json");
            using (var httpClient = new HttpClient())
            {
                var json = await httpClient.GetStringAsync(url);
                var wdata = JArray.Parse(json);                
                dynamic data = wdata.SelectToken("[0].data.children[0].data");
                string path = data.SelectToken("secure_media.reddit_video")?.fallback_url;
                media.Thumbnail = data.thumbnail;
                if (path == null)
                {
                    path = data.url_overridden_by_dest;
                }

                Uri uri = new Uri(path);
                if (uri.Host.Contains("youtu.be") || uri.Host.Contains("youtube.com"))
                {
                    if (uri.Host.Contains("youtube.com"))
                    {
                        media.ContentUrl = uri.AbsoluteUri;
                        path = HttpUtility.ParseQueryString(uri.Query).Get("v");
                    }
                    else
                    {
                        path = uri.AbsolutePath.Remove(0, 1);                        
                    }
                    media.Thumbnail = $"https://i.ytimg.com/vi/{path}/0.jpg";
                    media.ContentUrl = $"https://www.youtube.com/watch?v={path}";
                    source = SourceFromEnum.Youtube;
                    mediaType = MediaTypeEnum.Video;
                }
                if (uri.Host.Contains("v.redd.it"))
                {
                    media.SourceFrom = SourceFromEnum.Reddit;
                    //If it's an image
                    if (uri.AbsolutePath.IndexOf(".") != -1 && uri.AbsolutePath.Substring(uri.AbsolutePath.IndexOf(".")) == ".jpg")
                    {
                        path = uri.AbsolutePath.Remove(uri.AbsolutePath.Length - 4, 4).Remove(0, 1);
                        media.MediaType = MediaTypeEnum.Image;
                    }
                    else
                    {
                        path = uri.AbsolutePath.Remove(0, 1);
                        media.ContentUrl = $"https://v.redd.it/{path}";
                    }
                    mediaType = MediaTypeEnum.Video;
                }
                if (uri.Host == "i.redd.it")
                {
                    mediaType = MediaTypeEnum.Image;
                }
                if (mediaType != media.MediaType)
                    media.IsValid = false;
                else
                    media.IsValid = true;

                media.sourcePath = path;
                media.SourceFrom = source;
                return media;
            }
        }
        public static async Task<ViewMedia> PrepareStoreData(ViewMedia media)
        {
            if (media.Processed)
                return media;

            Uri myUri = new Uri(media.OriginalUrl);
            string host = myUri.Host;
            if (media.SourceFrom == SourceFromEnum.Youtube)
            {
                if (host.Contains("youtube.com"))
                {
                    Uri uri = new Uri(media.SourcePath, UriKind.Absolute);
                    media.sourcePath = HttpUtility.ParseQueryString(uri.Query).Get("v"); //only stores youtube's video id
                    return media;
                }
            }
            if (media.SourceFrom == SourceFromEnum.Reddit)
            {
                if (media.MediaType == MediaTypeEnum.Image)  //directly in image
                {
                    if (host.Contains("preview.redd.it"))
                    {
                        media.SourceFrom = SourceFromEnum.Link;
                        return media;
                    }
                    if (host.Contains("i.redd.it"))
                    {
                        Uri uri = new Uri(media.SourcePath);
                        media.sourcePath = uri.AbsolutePath.Remove(uri.AbsolutePath.Length - 4, 4).Remove(0, 1);
                        return media;
                    }
                }
                if (host.Contains("reddit.com")) //for everything in the comments page
                {
                    media = await GetRedditDataAsync(media);
                    return media;
                }
            }

            return media;
        }
    }
}
