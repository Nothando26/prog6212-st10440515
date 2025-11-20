using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace prog6212_st10440515_poe.Models
{
    public class Claim
    {
        [Key]
        public int ClaimID { get; set; }

        [Required]
        public int LecturerID { get; set; }

        [ForeignKey("LecturerID")]
        public Lecturer Lecturer { get; set; }

        [Required]
        [Range(0.1, 180, ErrorMessage = "Hours worked must be between 0.1 and 180.")]
        public double HoursWorked { get; set; }

        [Required]
        public double Amount { get; set; }

        public string SupportingDocumentPath { get; set; }

        public string SupportingDocumentName { get; set; } // optional

        public string Status { get; set; } = "Pending";

        public string CoordinatorReview { get; set; } = "Pending";
        public string ManagerReview { get; set; } = "Pending";

        public string Description { get; set; }

        public DateTime DateSubmitted { get; set; } = DateTime.Now;
    }
}