using Microsoft.AspNetCore.Http;
using Syncfusion.Blazor.Inputs;
using System.ComponentModel.DataAnnotations;
using Syncfusion.Blazor.Inputs.Internal;
using DataAccessLibrary.Enums;
using System.Threading.Tasks;
using static DataAccessLibrary.Enums.MediaEnums;
using System;
using System.Web;
using ThinBlueLie.Helper.Algorithms.WebsiteProfiling;

namespace ThinBlueLie.Models
{
    public class ViewMedia
    {
        public int IdMedia { get; set; }
        public int IdTimelineinfo { get; set; }
        public int? SubmittedBy { get; set; }
        public int Rank { get; set; } //becomes Rank
        [Required]
        public MediaTypeEnum? MediaType { get; set; }


        public string sourcePath;

        public string SourcePath
        {
            get { return sourcePath; }
            set { sourcePath = value;
                new Task(async () =>
                {
                    await GetData(this);
                }).Start();
            }
        }

        [Url]
        private string originalUrl;
        [Url]
        [MaxLength(500, ErrorMessage = "Please enter a url than 500 characters")]
        public string OriginalUrl
        {
            get { return originalUrl; }
            set { originalUrl = value;
                new Task(async () =>
                {
                    await GetData(this);
                }).Start();
            }
        }

        //For linked image 
        public string Thumbnail { get; set; } //link to thumbnail
        public string ContentUrl { get; set; } //link to video
        public string DisplayUrl { get; set; } //link to display       
        public bool Processed { get; set; }
        public UploadFiles Source { get; set; } //For uploaded image file         
        [Required]
        public byte Gore { get; set; }
        [Required(ErrorMessage = "Select where the Media is from")]
        public SourceFromEnum? SourceFrom { get; set; }
        [Required]
        [StringLength(250, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 10)]
        public string Blurb { get; set; }
        [MaxLength(100, ErrorMessage = "Please enter less than 100 characters")]
        public string Credit { get; set; }

        public static async Task<ViewMedia> GetData(ViewMedia media)
        {
            if (string.IsNullOrWhiteSpace(media.OriginalUrl))
            {
                return media; // break
            }

            if ((media.IdMedia != 0 && media.IdTimelineinfo != 0) || media.Thumbnail != null)
                media.Processed = true;
           
            if (media.Processed == false)
            {
                Uri uri = new Uri(media.OriginalUrl);
                if (uri.Host.Contains("youtube") || uri.Host.Contains("youtu.be"))
                {
                    media.SourceFrom = SourceFromEnum.Youtube;
                }
                if (uri.Host.Contains("reddit.com") || uri.Host.Contains("i.redd.it"))
                {
                    media.SourceFrom = SourceFromEnum.Reddit;
                }
            }
            if (media.MediaType == MediaTypeEnum.Video)
            {
                if (media.SourceFrom == SourceFromEnum.Youtube)
                {
                    // either https://youtu.be/hWLjYJ4BzvI or https://www.youtube.com/watch?v=YbgnlkJPga4
                    
                    if (media.Processed == false)
                    {
                        Uri uri = new Uri(media.OriginalUrl);
                        if (uri.Host.Contains("youtu.be"))
                        {
                            media.sourcePath = uri.AbsolutePath.Remove(0, 1);
                        }
                        if (uri.Host.Contains("youtube.com"))
                        {
                            media.sourcePath = HttpUtility.ParseQueryString(uri.Query).Get("v");
                        }                        
                    }
                    media.ContentUrl = $"https://www.youtube-nocookie.com/embed/{media.SourcePath}?rel=0&enablejsapi=1&iv_load_policy=3&version=3&modestbranding=1";
                    media.DisplayUrl = $"https://www.youtube.com/watch?v={media.SourcePath}";
                    media.Thumbnail = $"https://i.ytimg.com/vi/{media.SourcePath}/0.jpg";
                    media.Processed = true;
                    return media;
                }
                if (media.SourceFrom == SourceFromEnum.Reddit)
                {
                    if (media.Processed)
                    {
                        // -> https://v.redd.it/4ymh7g5fzfv51
                        media.ContentUrl = media.DisplayUrl = $"https://v.redd.it/{media.SourcePath}DASH_720.mp4";
                        return media;
                    }
                    else
                    {
                        var newMedia = await WebsiteProfile.GetRedditDataAsync(media);
                        newMedia.originalUrl = newMedia.ContentUrl;
                        await GetData(newMedia);
                    }
                }
            }           
          
            else if (media.MediaType == MediaTypeEnum.Image)
            {
                if (media.SourceFrom == SourceFromEnum.Device)
                {
                    if (media.Processed)
                    {
                        media.ContentUrl = media.Thumbnail = "/Uploads/" + media.SourcePath + ".jpg";                       
                    }
                    media.ContentUrl = media.Thumbnail = null;
                    return media;
                }
                else if (media.SourceFrom == SourceFromEnum.Reddit)
                {                    
                    if (media.MediaType == MediaTypeEnum.Image)
                    {
                        if (media.Processed == false)
                        {
                            Uri uri = new Uri(media.OriginalUrl);
                            if (uri.Host.Contains("preview.redd.it"))
                            {
                                media.SourceFrom = SourceFromEnum.Link;
                                media.ContentUrl = media.OriginalUrl;  //can't simplify link
                                media.Processed = true;
                                return media;
                            }
                            if (uri.Host.Contains("i.redd.it"))
                            {
                                media.sourcePath = uri.AbsolutePath.Remove(uri.AbsolutePath.Length - 4, 4).Remove(0, 1);
                                media.ContentUrl = media.Thumbnail = $"https://i.redd.it/{media.SourcePath}.jpg";
                                media.Processed = true;
                                return media;
                            }
                            if (uri.Host.Contains("reddit.com")) // in comments
                            {
                                var newMedia = await WebsiteProfile.GetRedditDataAsync(media);
                                media.Processed = true;
                                media.ContentUrl = newMedia.ContentUrl;
                                return media;
                            }
                        }
                        else
                        {
                            media.ContentUrl = $"https://i.redd.it/{media.SourcePath}.jpg";
                        }
                       
                    }
                }
                else
                {
                    media.ContentUrl = media.Thumbnail = media.SourcePath;
                    media.Processed = true;
                    return media;
                }
            }
            if (media.MediaType == MediaTypeEnum.News)
            {
                if (media.Processed == false)
                {
                    var news = await MetaScraper.GetMetaData(media.SourcePath);
                    media.Thumbnail = news.Image;
                    media.ContentUrl = media.sourcePath = media.originalUrl;
                }
                else
                {
                    media.originalUrl = media.SourcePath;
                }
                return media;
            }
            media.Processed = false;
            return media;
        }
    }
}
