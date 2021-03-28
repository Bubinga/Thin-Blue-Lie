using DataAccessLibrary.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLie.Helper.Validators;

namespace ThinBlueLie.Models
{
    public class ViewMisconduct
    {
        public int IdTimelineinfo { get; set; }
        public ViewOfficer Officer { get; set; }
        public ViewSubject Subject { get; set; }
        [Required]
        [MisconductValidator]
        public int[]? Misconduct { get; set; }
        [WeaponValidator("Misconduct", ErrorMessage = "'Unnecessary Use of Force' is selected, enter at least one weapon")]
        public int[]? Weapon { get; set; }
        public byte Armed { get; set; }
    }
}
