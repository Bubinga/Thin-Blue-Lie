using static DataAccessLibrary.Enums.TimelineinfoEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLibrary.Helper;
using System.Reflection;
using static ThinBlueLie.Models.ViewModels.TimelineinfoFull;
using ThinBlueLie.Models.ViewModels;

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
        public static string SeverityCalculatorManyHighest(List<TimelineinfoFull> timelineinfos)
        {
            int highestScore = 0;
            foreach (var timelineinfo in timelineinfos)
            {
                var result = SeverityCalculatorList(timelineinfo.OfficerInfo);
                if (result > highestScore)
                    highestScore = result;
            }
            if (highestScore > 0)
                return GetSeverityColor(highestScore);
            return "no-event";         
        }

        public static int SeverityCalculatorList(List<TimelineinfoOfficerShort> officers)
        {
            int score = 0;
            foreach (var officer in officers)
            {
                score += SeverityCalculator(officer.Misconduct, officer.Weapon);
            }
            return score;
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
            return score switch
            {
                int n when n >= 150 => "extreme",
                int n when n >= 100 => "high",
                int n when n >= 50 => "high-medium",
                int n when n >= 30 => "medium",
                int n when n >= 15 => "low-medium",
                _ => "low",
            };
        }
    }
}
