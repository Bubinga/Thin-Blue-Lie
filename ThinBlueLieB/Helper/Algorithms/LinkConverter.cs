using DataAccessLibrary.Enums;
using Syncfusion.Blazor.Gantt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLieB.Models;

namespace ThinBlueLieB.Helper.Algorithms
{
    public class LinkConverter
    {
        public static string GetLinkFromData(ViewMedia media, bool video = false)
        {
            if (media.MediaType == MediaEnums.MediaTypeEnum.Video)
            {
                if (media.SourceFrom == MediaEnums.SourceFromEnum.Youtube)
                {
                    if (video)
                    {
                        var path = $"https://www.youtube-nocookie.com/embed/{media.SourcePath}?rel=0&autoplay=1&enablejsapi=1&fs=0&iv_load_policy=3&version=3&modestbranding=1";
                        return path;
                    }
                    else
                    {
                        var path = $"https://i.ytimg.com/vi/{media.SourcePath}/0.jpg";
                        return path;
                    }
                } 
                //Add support for reddit
                //Add support for other sourcefroms
            }
            if (media.MediaType == MediaEnums.MediaTypeEnum.Image)
            {
                if (media.SourceFrom == MediaEnums.SourceFromEnum.Device)
                {
                    var path = @"C:\Programming\Projects\ThinBlueLieSolution\ThinBlueLieB\Uploads\" + media.SourcePath;
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
