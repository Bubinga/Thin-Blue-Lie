using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static DataAccessLibrary.Enums.TimelineinfoEnums;

namespace ThinBlueLieB.Helper.Extensions
{
    public static class EnumExtensions      
    {     

        public static List<ListItem> GetEnumDisplayNames<T>() where T : Enum
        {
            var type = typeof(T);
            return Enum.GetValues(type)
                       .Cast<T>()
                       .Select(x => new ListItem
                       {
                           Text = type.GetMember(x.ToString()).First().GetCustomAttribute<DisplayAttribute>()?.Name ?? x.ToString(),
                           Value = x
                       })
                   .ToList();
        }

        public static string GetEnumDisplayName<T>(T value) where T : Enum
        {
            if (value == null)
            {
                return string.Empty;
            }
            string fieldName;
            DisplayAttribute displayAttr;
            try
            {
                fieldName = Enum.GetName(typeof(T), value);
                displayAttr = typeof(T)
               .GetField(fieldName)
               .GetCustomAttribute<DisplayAttribute>();
            }
            catch (ArgumentException)
            {
                fieldName = Enum.GetName(value.GetType(), value);
                displayAttr = value.GetType()
               .GetField(fieldName)
               .GetCustomAttribute<DisplayAttribute>();
            }
            return displayAttr?.Name ?? fieldName;
        }

        public class ListItem
        {
            public dynamic Value { get; set; }
            public string Text { get; set; }
        }
        public static class GetDropdownList<TEnum>
         where TEnum : struct, Enum
        {
            public static IReadOnlyList<ListItem> Items { get; } = Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
                            .Select(e => new ListItem() { Text = GetEnumDisplayName(e), Value = Convert.ToInt32(e) }).ToList();
        }

        public static ArmedEnum ToArmedEnum(bool boolean)
        {
            if (boolean)
            {
                return ArmedEnum.Armed;
            }
            else
            {
                return ArmedEnum.Unarmed;
            }
        }
    }
}
