using System.ComponentModel.DataAnnotations;

namespace prog6212_st10440515_poe.Models.ViewModels
{
    public class CreateUserViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; } // "Lecturer", "Coordinator", "Manager", "HR"
    }
}
