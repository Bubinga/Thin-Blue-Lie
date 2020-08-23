using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Components;

namespace ThinBlueLie.Models
{
    public static class Extensions
    {
        public static string GetEnumDisplayName<T>(T value) where T : Enum
        {
            var fieldName = Enum.GetName(typeof(T), value);
            var displayAttr = typeof(T)
                .GetField(fieldName)
                .GetCustomAttribute<DisplayAttribute>();
            return displayAttr?.Name ?? fieldName;
        }
        public static string TruncateString(string str)
        {
            const int MaxLength = 100;
            var name = str;
            if (name.Length > MaxLength)
            {
                name = name.Substring(0, MaxLength);
                name = name + "...";
            }
            return name;
        }
        [Inject]
        public static NavigationManager MyNavigationManager { get; set; }
        public static string GetQueryParm(string parmName)
        {
            var uriBuilder = new UriBuilder(MyNavigationManager.Uri);
            var q = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
            return q[parmName] ?? "";
        }
    }
}
