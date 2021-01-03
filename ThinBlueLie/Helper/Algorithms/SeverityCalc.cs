using static DataAccessLibrary.Enums.TimelineinfoEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLibrary.Helper;
using System.Reflection;
using static ThinBlueLie.Models.ViewModels.TimelineinfoFull;

namespace ThinBlueLie.Helper.Algorithms
{
    public class SeverityCalc
    {
        //misconduct type
        //weapon used
        //generate score 1-100
        public static int GetValue<T>(T value) where T : Enum
        {
            var fieldName = Enum.GetName(typeof(T), value);
            var displayAttr = typeof(T)
                .GetField(fieldName)
                .GetCustomAttribute<ValueAttribute>();
            return displayAttr.Value;
        }

        public static string SeverityCalculatorMany(List<TimelineinfoOfficerShort> officers)
        {
            int score = 0;
            foreach (var officer in officers)
            {
                score += SeverityCalculator(officer.Misconduct, officer.Weapon);
            }
            return GetSeverityColor(score);
        }

        public static int SeverityCalculator(MisconductEnum misconduct, WeaponEnum weapon)
        {
            int score = 0;
            foreach (MisconductEnum value in Enum.GetValues(typeof(MisconductEnum)))
            {
                if (misconduct.HasFlag(value))
                {
                    score += GetValue(value);
                }
            }
            foreach (WeaponEnum value in Enum.GetValues(typeof(WeaponEnum)))
            {
                if (weapon.HasFlag(value))
                {
                    score += GetValue(value);
                }
            }
            return score;
        }
        public static string GetSeverityColor(int score)
        {
            switch (score)
            {
                case int n when n >= 150:
                    return "--extreme";
                case int n when n >= 100:
                    return "--high";
                case int n when n >= 50:
                    return "--high-medium";
                case int n when n >= 30:
                    return "--medium";
                case int n when n >= 15:
                    return "--low-medium";

                default:
                    return "--low";
            }
        }
    }
}
