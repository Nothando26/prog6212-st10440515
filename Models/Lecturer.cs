using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace prog6212_st10440515_poe.Models
{
    public class Lecturer
    {
        [Key]
        public int LecturerID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public double HourlyRate { get; set; }

        public double MaxHoursPerMonth { get; set; } = 180;

        [NotMapped]
        public string FullName => $"{Name} {Surname}";

        public ICollection<Claim> Claims { get; set; } = new List<Claim>();
    }

}
 
