using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prog6212_st10440515_poe.Data;
using prog6212_st10440515_poe.Models;
using System;
using System.IO;
using System.Linq;

namespace prog6212_st10440515_poe.Controllers
{
    public class LecturerController : Controller
    {
        private readonly AppDbContext _context;

        public LecturerController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Lecturer Dashboard
        public IActionResult LecturerDashboard()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var roleClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            if (userIdClaim == null || roleClaim != "Lecturer")
                return RedirectToAction("Login", "Account");

            var userId = int.Parse(userIdClaim);

            var lecturer = _context.Lecturers
                .Include(l => l.Claims)
                .SingleOrDefault(l => l.UserID == userId);

            if (lecturer == null)
            {
                var user = _context.Users.Find(userId);
                if (user == null) return RedirectToAction("Login", "Account");

                lecturer = new Lecturer
                {
                    UserID = user.UserID,
                    Name = user.FullName.Split(' ')[0],
                    Surname = string.Join(' ', user.FullName.Split(' ').Skip(1)),
                    Email = user.Email,
                    HourlyRate = 0
                };

                _context.Lecturers.Add(lecturer);
                _context.SaveChanges();
            }

            var claims = lecturer.Claims.OrderByDescending(c => c.DateSubmitted).ToList();
            return View(claims);
        }

        // GET: Claim Form
        public IActionResult ClaimForm()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) return RedirectToAction("Login", "Account");

            var userId = int.Parse(userIdClaim);
            var lecturer = _context.Lecturers.SingleOrDefault(l => l.UserID == userId);
            if (lecturer == null) return RedirectToAction("LecturerDashboard");

            return View(lecturer);
        }

        // POST: Submit Claim
        [HttpPost]
        public IActionResult ClaimForm(double hoursWorked, string notes, IFormFile supportingDocs)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) return RedirectToAction("Login", "Account");

            var userId = int.Parse(userIdClaim);
            var lecturer = _context.Lecturers.FirstOrDefault(l => l.UserID == userId);

            if (lecturer == null)
            {
                TempData["Error"] = "Lecturer not found.";
                return RedirectToAction("LecturerDashboard");
            }

            // Check monthly hour limit
            var currentMonthHours = _context.Claims
                .Where(c => c.LecturerID == lecturer.LecturerID &&
                            c.DateSubmitted.Month == DateTime.Now.Month &&
                            c.DateSubmitted.Year == DateTime.Now.Year)
                .Sum(c => c.HoursWorked);

            if (currentMonthHours + hoursWorked > 180)
            {
                TempData["Error"] = "You cannot submit more than 180 hours this month.";
                return RedirectToAction("ClaimForm");
            }

            // Handle document
            string docPath = null;
            if (supportingDocs != null)
            {
                var allowedExtensions = new[] { ".pdf", ".docx", ".xlsx" };
                var ext = Path.GetExtension(supportingDocs.FileName).ToLower();

                if (!allowedExtensions.Contains(ext))
                {
                    TempData["Error"] = "Only PDF, DOCX, or XLSX files allowed.";
                    return RedirectToAction("ClaimForm");
                }

                if (supportingDocs.Length > 5 * 1024 * 1024)
                {
                    TempData["Error"] = "File size exceeds 5 MB limit.";
                    return RedirectToAction("ClaimForm");
                }

                var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

                docPath = Path.Combine("uploads", Guid.NewGuid() + "_" + supportingDocs.FileName);
                var fullPath = Path.Combine("wwwroot", docPath);

                using var stream = new FileStream(fullPath, FileMode.Create);
                supportingDocs.CopyTo(stream);
            }

            var claim = new Claim
            {
                LecturerID = lecturer.LecturerID,
                HoursWorked = hoursWorked,
                Amount = hoursWorked * lecturer.HourlyRate,
                DateSubmitted = DateTime.Now,
                Description = notes,
                SupportingDocumentPath = docPath,
                SupportingDocumentName = supportingDocs?.FileName,
                CoordinatorReview = "Pending",
                ManagerReview = "Pending",
                Status = "Pending"
            };

            _context.Claims.Add(claim);
            _context.SaveChanges();

            TempData["Success"] = "Claim submitted successfully!";
            return RedirectToAction("LecturerDashboard");
        }
    }
}

