using System.ComponentModel.DataAnnotations;

namespace DataAccessLibrary.Enums
{
    public class FlaggedEnums
    {
        public enum FlagTypeEnum { 
            [Display(Name = "Vulgar or Racist")]
            Vulgar = 1,
            [Display(Name = "False Information")]
            False,
            [Display(Name = "Not Police Brutality")]
            NotBrutality,
            [Display(Name = "Duplicate Event")]
            Duplicate,
            Other
        }
    }
}
