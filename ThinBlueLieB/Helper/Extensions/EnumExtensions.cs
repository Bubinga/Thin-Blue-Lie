using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ThinBlueLieB.Helper.Extensions
{
    public static class EnumExtensions      
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }
        public static List<string> GetEnumDisplayNameList<T>(T value) where T : Enum
        {
            List<string> nameList = new List<string>();
            foreach (T Value in Enum.GetValues(typeof(T)))
            {
                nameList.Add(Value.GetDisplayName());
                //var fieldName = Enum.GetName(typeof(T), value);
                //var displayAttr = typeof(T)
                //    .GetField(fieldName)
                //    .GetCustomAttribute<DisplayAttribute>();
                //var name = displayAttr?.Name ?? fieldName;
                //nameList.Add(name);
            }
            return nameList;

        }

        public static string GetEnumDisplayName<T>(T value) where T : Enum
        {
            var fieldName = Enum.GetName(typeof(T), value);
            var displayAttr = typeof(T)
                .GetField(fieldName)
                .GetCustomAttribute<DisplayAttribute>();
            return displayAttr?.Name ?? fieldName;
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
    }
    public static class EnumHelper<T>
    where T : struct, Enum // This constraint requires C# 7.3 or later.
    {
        public static IList<T> GetValues(Enum value)
        {
            var enumValues = new List<T>();

            foreach (FieldInfo fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumValues.Add((T)Enum.Parse(value.GetType(), fi.Name, false));
            }
            return enumValues;
        }

        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static IList<string> GetNames(Enum value)
        {
            return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }

        public static IList<string> GetDisplayValues(Enum value)
        {
            return GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
        }

        private static string lookupResource(Type resourceManagerProvider, string resourceKey)
        {
            foreach (PropertyInfo staticProperty in resourceManagerProvider.GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
            {
                if (staticProperty.PropertyType == typeof(System.Resources.ResourceManager))
                {
                    System.Resources.ResourceManager resourceManager = (System.Resources.ResourceManager)staticProperty.GetValue(null, null);
                    return resourceManager.GetString(resourceKey);
                }
            }

            return resourceKey; // Fallback with the key name
        }

        public static string GetDisplayValue(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes[0].ResourceType != null)
                return lookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name);

            if (descriptionAttributes == null) return string.Empty;
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
        }
    }
}
