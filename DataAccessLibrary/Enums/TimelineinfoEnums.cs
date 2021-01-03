using System;
using System.ComponentModel.DataAnnotations;
using DataAccessLibrary.Helper;

namespace DataAccessLibrary.Enums
{
    public static class TimelineinfoEnums
    {      
        public enum ArmedEnum
        {
            Unarmed, 
            Armed
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
            [Display(Name = "Unnecessary Use of Force")]
            [Value(20)]
            Force = 1,
            [Display(Name = "Murder of Pet")]
            [Value(60)]
            PetMurder = 2,
            [Display(Name = "Evidence Tampering")]
            [Value(25)]
            Evidence = 4,
            [Display(Name = "Non-Violent Harassment")]
            [Value(10)]
            Harassment = 8,
            [Value(10)]
            Negligence = 16,
            [Display(Name = "Unlawful Seizure")]
            [Value(15)]
            Theft = 32,
            [Display(Name = "Unlawful Search")]
            [Value(12)]
            Search = 64,
            [Display(Name = "False Arrest")]
            [Value(13)]
            FalseArrest = 128,
            [Display(Name = "Abuse of Power")]
            [Value(7)]
            AbusePower = 256,
        }

        [Flags]
        public enum WeaponEnum : short
        {
            [Display(Name = "Bodily Force")]
            [Value(5)]
            Body = 1,
            [Display(Name = "Handheld Weapon")]
            [Value(7)]
            HandWeapon = 2,
            [Display(Name = "Nonlethal Projectile")]
            [Value(10)]
            Projectile = 4,
            [Value(10)]
            Taser = 8,
            [Display(Name ="Tear Gas")]
            [Value(5)]
            TearGas = 16,
            [Value(14)]
            Vehicle = 32,
            [Value(45)]
            Gun = 64
        }

    }
}
