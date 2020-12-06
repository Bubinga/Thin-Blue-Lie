using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLie.Models;
using DataAccessLibrary.Enums;
using Syncfusion.Blazor.CircularGauge.Internal;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;

namespace ThinBlueLie.Helper.Algorithms.WebsiteProfiling
{
    public partial class WebsiteProfile
    {
        public async Task<string> PrepareStoreData(ViewMedia media)
        {
            string path; 
            if (media.SourceFrom == MediaEnums.SourceFromEnum.Youtube)
            {
                Uri uri = new Uri(media.SourcePath, UriKind.Absolute);
                path = HttpUtility.ParseQueryString(uri.Query).Get("v"); //only stores youtube's video id
                return path;
            }
            if (media.SourceFrom == MediaEnums.SourceFromEnum.Reddit)
            {
                Uri myUri = new Uri(media.SourcePath);
                string host = myUri.Host;
                if (host.Contains("reddit.com")) //in comments not in image
                {
                    //download json data and search for url_overridden_by_dest
                    var jsonPath = media.SourcePath.Remove(media.SourcePath.Length - 1, 1).Insert(media.SourcePath.Length -1 , ".json");
                    using (var httpClient = new HttpClient())
                    {
                        var json = await httpClient.GetStringAsync(jsonPath);
                        dynamic stuff = JsonConvert.SerializeObject(json);
                        path = stuff.url_overridden_by_dest;
                        Uri uri = new Uri(path);
                        path = uri.AbsolutePath.Remove(uri.AbsolutePath.Length - 4, 4).Remove(0,1);
                        return path; //string of characters that go into format of -> https://i.redd.it/0u3pdpo3zgv51.jpg
                    }
                }
                else if (media.SourcePath.Contains("preview.redd.it")) //in image
                {
                    Uri uri = new Uri(media.SourcePath);
                    path = uri.AbsolutePath.Remove(uri.AbsolutePath.Length - 4, 4).Remove(0, 1);
                    return path;
                }
            }
            return media.SourcePath;
        }
    }
}
