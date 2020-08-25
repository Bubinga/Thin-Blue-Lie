using System.ComponentModel.DataAnnotations;

namespace ThinBlueLieB.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z ]*$")]
        [MaxLength(25)]
        public string Username { get; set; }
    }
}
