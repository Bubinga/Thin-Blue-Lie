using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using DataAccessLibrary.Enums;
using Microsoft.AspNetCore.Components;
using ThinBlueLieB.Models;
using static ThinBlueLieB.Models.ViewSimilar;

namespace ThinBlueLieB.Helper.Extensions
{
    public class StringExtensions
    {        

        public static string TruncateString(string str, int MaxLength = 100)
        {
            var name = str;
            if (name.Length > MaxLength)
            {
                name = name.Substring(0, MaxLength);
                name += "...";
            }
            return name;
        }

        static string ComparePersonProperties(object oldProperty, object newProperty, bool IsOriginal)
        {
            string summary = string.Empty;
            if (newProperty?.Equals(oldProperty) == oldProperty?.Equals(newProperty))
                if (newProperty == null)
                    summary += "Unknown";                
                else                
                    summary += newProperty.ToString();                
            else
            {
                if (IsOriginal)
                    summary += "<del>" + oldProperty + "</del> ";
                else
                    summary += "<ins>" + newProperty + "</ins> ";
            }
            return summary;
        }
        public static string SimpleCompare(string? Old, string? New, bool IsOriginal)
        {
            if (Old == New)
            {
                return Old;
            }
            else
            {
                if (IsOriginal)
                {
                    return "<del>" + Old + "</del>";
                }
                else
                {
                    return "<ins>" + New + "</ins>";
                }
            }
        }
        public static string GetComparePersonSummary(CommonPerson oldPerson, CommonPerson newPerson, bool IsOriginal)
        {
            string summary = string.Empty;
            summary += ComparePersonProperties(oldPerson?.Race, newPerson?.Race, IsOriginal) + " ";
            summary += ComparePersonProperties(oldPerson?.Sex, newPerson?.Sex, IsOriginal) + ", ";
            summary += ComparePersonProperties(oldPerson?.Age, newPerson?.Age, IsOriginal) + " years old";
            return summary;
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
        internal static List<string> GetSummaryList(List<ViewSimilarPerson> People)
        {
            List<string> list = new List<string>();
            foreach (var person in People)
            {
                list.Add(StringExtensions.GetPersonSummary(person));
            }
            return list;
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

        private static readonly Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
