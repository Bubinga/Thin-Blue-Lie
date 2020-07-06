using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ThinBlue
{
    public partial class Timelineinfo
    {        
        public int IdTimelineInfo { get; set; }
        [Required]
        [Column(TypeName = "DATETIME()")]
        public string Date { get; set; }
        [Required]
        [Column(TypeName = "VARCHAR(60)")]
        public string State { get; set; }
        [Required]
        [MaxLength(86)]
        [Column(TypeName = "VARCHAR(20)")]
        public string City { get; set; }
        [MaxLength(60)]
        [Column(TypeName = "VARCHAR(60)")]
        public string SubjectName { get; set; }
        [Required]
        [Column(TypeName = "BIT(2)")]
        public int SubjectSex { get; set; }
        [Column(TypeName = "TINYINT(1)")]
        public int SubjectRace { get; set; }
        [Column(TypeName = "BIT(1)")]
        public int Armed { get; set; }
        [MaxLength(60)]
        [Column(TypeName = "VARCHAR(60)")]
        public string OfficerName { get; set; }
        [Column(TypeName = "BIT(2)")]
        public int OfficerSex { get; set; }
        [Column(TypeName = "TINYINT(1)")]
        public int OfficerRace { get; set; }
        [Column(TypeName = "TINYINT(3)")]
        public int Misconduct { get; set; }
        [Column(TypeName = "TINYINT(2)")]
        public int Weapon { get; set; }
        [Column(TypeName = "LONGTEXT")]
        public string Context { get; set; }
        [Required]
        [Column(TypeName = "BIT(1)")]
        public int Gore { get; set; }
        [Column(TypeName = "TINYINT(1)")]
        public int Source { get; set; }
        [Column(TypeName = "VARCHAR(60)")]
        public string Credit { get; set; }
        [Column(TypeName = "VARCHAR(100)")]
        public string VidLink { get; set; }

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

        
        public enum RaceEnum :short
        {
            Unclear,
            White,
            Black,
            Hispanic,
            Asian,
            Native,
            Multiracial
        }
        [Flags]
        public enum MisconductEnum : short
        {
            Force = 0b_0000_0001,
            PetMurder = 0b_0000_0001,
            Evidence = 0b_0000_1000,
            Harassment = 0b_0010_0000,
            Negligence = 0b_0100_0000,
            Theft = 0b_0001_0000,
            Search = 0b_0000_0001,
            FalseArrest = 0b_0000_0100,
            AbusePower = 0b_0000_0001,
        }

        [Flags]
        public enum WeaponEnum : short
        {
            Body       = 0b_0000_0001,
            Projectile = 0b_0000_0010,
            Taser      = 0b_0000_0100,
            TearGas    = 0b_0000_1000,
            Vehicle    = 0b_0001_0000,
            Gun        = 0b_0010_0000
        }

        public enum SourceEnum : short
        {
            Youtube,
            Reddit,
            Instagram,
            Facebook,
            PhoneComputer,
            Other,
        }

        //public static string GetSex(Sex sex)
        //{
        //    switch (sex)
        //    {
        //        case Sex.Male:
        //            return "Male";
        //        case Sex.Female:
        //            return "Female";
        //        default:
        //            return "N/A";
        //    }
        //}
    }
}
