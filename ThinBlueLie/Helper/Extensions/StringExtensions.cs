using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using DataAccessLibrary.Enums;
using KellermanSoftware.CompareNetObjects;
using Microsoft.AspNetCore.Components;
using ThinBlueLie.Models;
using static ThinBlueLie.Models.ViewSimilar;

namespace ThinBlueLie.Helper.Extensions
{
    public class StringExtensions
    {
        internal static string NormalizeWhiteSpace(string input, char normalizeTo = ' ')
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            int current = 0;
            char[] output = new char[input.Length];
            bool skipped = false;

            foreach (char c in input.ToCharArray())
            {
                if (char.IsWhiteSpace(c))
                {
                    if (!skipped)
                    {
                        if (current > 0)
                            output[current++] = normalizeTo;

                        skipped = true;
                    }
                }
                else
                {
                    skipped = false;
                    output[current++] = c;
                }
            }

            return new string(output, 0, skipped ? current - 1 : current);
        }
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

        public static string SimpleCompare(object Old, object New, bool IsOriginal)
        {
            string summary = string.Empty;
            CompareLogic compareLogic = new CompareLogic();
            if (compareLogic.Compare(Old, New).AreEqual)
                if (New == null)
                    summary += "Unknown";                
                else                
                    summary += New.ToString();                
            else
            {
                if (New != null && Old == null) //If the event is new
                {
                    summary += New;
                }
                else if (IsOriginal)
                    summary += "<del>" + Old + "</del> ";
                else
                    summary += "<ins>" + New + "</ins> ";
            }
            return summary;
        }
       
        public static string GetComparePersonSummary(CommonPerson oldPerson, CommonPerson newPerson, bool IsOriginal, bool getName = false)
        {
            string summary = string.Empty;
            if (getName)
                summary += newPerson.Name + ", ";
            summary += SimpleCompare(oldPerson?.Race, newPerson?.Race, IsOriginal) + " ";
            summary += SimpleCompare(oldPerson?.Sex, newPerson?.Sex, IsOriginal) + ", ";
            summary += SimpleCompare(oldPerson?.Age, newPerson?.Age, IsOriginal) + (getName? "" : " years old");
            return summary;
        }

        internal static List<string> GetSummaryList(List<CommonPerson> People)
        {
            List<string> list = new List<string>();
            foreach (var person in People)
            {
                list.Add(GetComparePersonSummary(person, person, true, true));
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
