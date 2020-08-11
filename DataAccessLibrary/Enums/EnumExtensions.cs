using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DataAccessLibrary.Enums
{
    public static class EnumExtensions
    {
        public static string GetEnumDisplayName<T>(T value) where T : Enum
        {
            var fieldName = Enum.GetName(typeof(T), value);
            var displayAttr = typeof(T)
                .GetField(fieldName)
                .GetCustomAttribute<DisplayAttribute>();
            return displayAttr?.Name ?? fieldName;
        }
    }
}
