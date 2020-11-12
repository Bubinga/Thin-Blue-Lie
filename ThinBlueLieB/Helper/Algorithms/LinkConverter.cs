using DataAccessLibrary.DataModels;
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
        public static string GetLinkFromData(Media media, bool video = false)
        {
            if ((MediaEnums.MediaTypeEnum)media.MediaType == MediaEnums.MediaTypeEnum.Video)
            {
                if ((MediaEnums.SourceFromEnum)media.SourceFrom == MediaEnums.SourceFromEnum.Youtube)
                {
                    if (video)
                    {
                        var path = $"https://www.youtube-nocookie.com/embed/{media.SourcePath}?rel=0&enablejsapi=1&iv_load_policy=3&version=3&modestbranding=1";
                        return path;
                    }
                    else
                    {
                        var path = $"https://i.ytimg.com/vi/{media.SourcePath}/0.jpg";
                        return path;
                    }
                }
                //Add support for reddit
                // image: https://i.redd.it/0u3pdpo3zgv51.jpg
                //Add support for other sourcefroms
            }
            if ((MediaEnums.MediaTypeEnum)media.MediaType == MediaEnums.MediaTypeEnum.Image)
            {
                if ((MediaEnums.SourceFromEnum)media.SourceFrom == MediaEnums.SourceFromEnum.Device)
                {
                    var path = ConfigHelper.GetUploadsDirectory() + media.SourcePath;
                    return path;
                }
                else if ((MediaEnums.SourceFromEnum)media.SourceFrom == MediaEnums.SourceFromEnum.Reddit)
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
