using System.ComponentModel.DataAnnotations;

namespace prog6212_st10440515_poe.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required, MaxLength(50)]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; } // Store hashed password

        [Required, MaxLength(20)]
        public string Role { get; set; } // "Lecturer", "Coordinator", "Manager"
    }
}
