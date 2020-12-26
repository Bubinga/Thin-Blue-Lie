using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ThinBlueLie.Helper.Extensions
{
    public class UriExtensions
    {
        public static string RemoveQueryStringByKey(string url, string key)
        {
            var uri = new Uri(url);

            // this gets all the query string key value pairs as a collection
            var newQueryString = HttpUtility.ParseQueryString(uri.Query);

            // this removes the key if exists
            newQueryString.Remove(key);

            // this gets the page path from root without QueryString
            string pagePathWithoutQueryString = uri.GetLeftPart(UriPartial.Path);

            return newQueryString.Count > 0
                ? String.Format("{0}?{1}", pagePathWithoutQueryString, newQueryString)
                : pagePathWithoutQueryString;
        }
        public static string CreateTitleUrl(string url)
        {
            var title = new string(url.Where(c => !char.IsPunctuation(c)).ToArray()).Split(' ').Take(5);
            var shorttitle = string.Join("-", title);
            return shorttitle;
        }
    }
}
