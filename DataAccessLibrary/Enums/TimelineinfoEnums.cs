using System;
using System.ComponentModel.DataAnnotations;

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
            Force = 1,
            [Display(Name = "Murder of Pet")]
            PetMurder = 2,
            [Display(Name = "Evidence Tampering")]
            Evidence = 4,
            [Display(Name = "Non-Violent Harassment")]
            Harassment = 8,
            Negligence = 16,
            [Display(Name = "Unlawful Seizure")]
            Theft = 32,
            [Display(Name = "Unlawful Search")]
            Search = 64,
            [Display(Name = "False Arrest")]
            FalseArrest = 128,
            [Display(Name = "Abuse of Power")]
            AbusePower = 256,
        }

        [Flags]
        public enum WeaponEnum : short
        {
            Body = 1,
            [Display(Name = "Handheld Weapon")]
            HandWeapon = 2,
            Projectile = 4,
            Taser = 8,
            [Display(Name ="Tear Gas")]
            TearGas = 16,
            Vehicle = 32,
            Gun = 64
        }

    }
}
