using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using DataAccessLibrary.Enums;
using Microsoft.AspNetCore.Components;
using static ThinBlueLieB.Models.ViewSimilar;

namespace ThinBlueLieB.Helper
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
        public class ListItem
        {
            public int Value { get; set; }
            public string Text { get; set; }
        }
        public static class GetDropdownList<TEnum>
         where TEnum : struct, Enum
        {
            private static readonly IReadOnlyList<ListItem> _items = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Select(e => new ListItem() { Text = e.ToString(), Value = Convert.ToInt32(e) }).ToList();

            public static IReadOnlyList<ListItem> Items => _items;
        }
        internal static string GetPersonSummary(ViewSimilarPerson person)
        {
            var personSummary = person.Name + ", " + person.Age.ToString() + ", " + Enum.GetName(typeof(TimelineinfoEnums.RaceEnum), person.Race) 
                                + " " + Enum.GetName(typeof(TimelineinfoEnums.SexEnum), person.Sex);
            return personSummary;
        }
    }
}
