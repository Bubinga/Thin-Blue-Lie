using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using DataAccessLibrary.Enums;
using Microsoft.AspNetCore.Components;
using static ThinBlueLieB.Models.ViewSimilar;

namespace ThinBlueLieB.Helper
{
    public class Extensions
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
                name += "...";
            }
            return name;
        }

        [Inject]
        public NavigationManager MyNavigationManager { get; set; }
        public string GetQueryParm(string parmName)
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
            public static IReadOnlyList<ListItem> Items { get; } = Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
                            .Select(e => new ListItem() { Text = GetEnumDisplayName(e), Value = Convert.ToInt32(e) }).ToList();
        }
        internal static string GetPersonSummary(ViewSimilarPerson person)
        {
            var personSummary = person.Name;

            if (person.Age != null)
            {
                personSummary += ", " + person.Age.ToString();
            }
            personSummary += ", " + Enum.GetName(typeof(TimelineinfoEnums.RaceEnum), person.Race)
                               + " " + Enum.GetName(typeof(TimelineinfoEnums.SexEnum), person.Sex);
            return personSummary;
        }

        public static string CommaQuibbling(IEnumerable<string> items)
        {
            StringBuilder sb = new StringBuilder();
            using (var iter = items.GetEnumerator())
            {
                if (iter.MoveNext())
                { // first item can be appended directly
                    sb.Append(iter.Current);
                    if (iter.MoveNext())
                    { // more than one; only add each
                      // term when we know there is another
                        string lastItem = iter.Current;
                        while (iter.MoveNext())
                        { // middle term; use ", "
                            sb.Append(", ").Append(lastItem);
                            lastItem = iter.Current;
                        }
                        // add the final term; since we are on at least the
                        // second term, always use " and "
                        sb.Append(", and ").Append(lastItem);
                    }
                }
            }
            return sb.ToString();
        }

        public static string GetDaySuffix(int day)
        {
            switch (day)
            {
                case 1:
                case 21:
                case 31:
                    return "st";
                case 2:
                case 22:
                    return "nd";
                case 3:
                case 23:
                    return "rd";
                default:
                    return "th";
            }
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
