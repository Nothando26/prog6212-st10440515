using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prog6212_st10440515_poe.Models
{
    public class Lecturer
    {
        [Key]
        public int LecturerID { get; set; }

        [Required]
        public int UserID { get; set; } // FK to User

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public double HourlyRate { get; set; }

        // Navigation property for claims
        // NEW: navigation property
        public ICollection<Claim> Claims { get; set; } = new List<Claim>();
    }

        public class Claim
        {
            [Key]
            public int ClaimID { get; set; }

        [Required]
        public int LecturerID { get; set; }

        [ForeignKey("LecturerID")]
        public Lecturer Lecturer { get; set; }

        [Required]
            public double HoursWorked { get; set; }

            [Required]
            public double Amount { get; set; }

            public string SupportingDocumentPath { get; set; }

            public string Status { get; set; } = "Pending"; // General claim status

            public string CoordinatorReview { get; set; } = "Pending"; // Pending, Accepted, Rejected
            public string ManagerReview { get; set; } = "Pending"; // Pending, Accepted, Rejected, Verification

            public string Description { get; set; } // Additional notes

            public DateTime DateSubmitted { get; set; } = DateTime.Now;

         
        }
    }
