using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLibrary.Enums
{
    public static class TimelineinfoEnums
    {      
        public enum ArmedEnum
        {
            No, 
            Yes
        }

        public enum StateEnum
        {
            Alabama, Alaska, [Display(Name = "American Samoa")] AmericanSamoa, Arizona, Arkansas,
            California, Colorado, Connecticut, Delaware, [Display(Name = "District of Columbia")] DistrictofColumbia,
            Florida, Georgia, Guam, Hawaii, Idaho, Illinois, Indiana, Iowa, Kansas, Kentucky, Louisiana, Maine, Maryland,
            Massachusetts, Michigan, Minnesota, Mississippi, Missouri, Montana, Nebraska, Nevada,
            [Display(Name = "New Hampshire")] NewHampshire, [Display(Name = "New Jersey")] NewJersey,
            [Display(Name = "New Mexico")] NewMexico, [Display(Name = "New York")] NewYork, [Display(Name = "North Carolina")] NorthCarolina,
            [Display(Name = "North Dakota")] NorthDakota, [Display(Name = "Northern Mariana Islands")] NorthernMarianaIslands,
            Ohio, Oklahoma, Oregon, Pennsylvania, [Display(Name = "Puerto Rico")] PuertoRico,
            [Display(Name = "Rhode Island")] RhodeIsland, [Display(Name = "South Carolina")] SouthCarolina,
            [Display(Name = "South Dakota")] SouthDakota, Tennessee, Texas, [Display(Name = "Virgin Islands")] VirginIslands,
            Utah, Vermont, Virginia, Washington, [Display(Name = "West Virginia")] WestVirginia, Wisconsin, Wyoming
        }


        public enum SexEnum : short
        {
            Male,
            Female,
            Unclear
        }

        public enum RaceEnum : short
        {            
            White,
            Black,
            Hispanic,
            Asian,
            Native,
            Multiracial,
            Unclear
        }
        [Flags]
        public enum MisconductEnum : short
        {
            Force = 1,
            PetMurder = 2,
            Evidence = 4,
            Harassment = 8,
            Negligence = 16,
            Theft = 32,
            Search = 64,
            FalseArrest = 128,
            AbusePower = 256,
        }

        [Flags]
        public enum WeaponEnum : short
        {
            Body = 0b_0000_0001,
            Projectile = 0b_0000_0010,
            Taser = 0b_0000_0100,
            TearGas = 0b_0000_1000,
            Vehicle = 0b_0001_0000,
            Gun = 0b_0010_0000
        }

    }
}
