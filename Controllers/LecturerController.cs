using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prog6212_st10440515_poe.Data;
using prog6212_st10440515_poe.Models;

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
            var userId = HttpContext.Session.GetInt32("UserID");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null || !string.Equals(role, "Lecturer", StringComparison.OrdinalIgnoreCase))
                return RedirectToAction("Login", "Account");

            // Ensure Lecturer exists
            var lecturer = _context.Lecturers.SingleOrDefault(l => l.UserID == userId);
            if (lecturer == null)
            {
                var user = _context.Users.Find(userId);
                if (user == null)
                    return RedirectToAction("Login", "Account");

                lecturer = new Lecturer
                {
                    UserID = user.UserID,
                    Name = user.FullName.Split(' ')[0],
                    Surname = string.Join(' ', user.FullName.Split(' ').Skip(1)),
                    Email = user.Email,
                    HourlyRate = 0 // default
                };
                _context.Lecturers.Add(lecturer);
                _context.SaveChanges();
            }

            // Load claims with Lecturer details
            var claims = _context.Claims
                .Where(c => c.LecturerID == lecturer.LecturerID)
                .Include(c => c.Lecturer)
                .OrderByDescending(c => c.DateSubmitted)
                .ToList();

            ViewBag.Lecturer = lecturer;
            return View(claims);
        }

        // GET: Claim Form
        public IActionResult ClaimForm()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null || HttpContext.Session.GetString("Role") != "Lecturer")
                return RedirectToAction("Login", "Account");

            var lecturer = _context.Lecturers.SingleOrDefault(l => l.UserID == userId);
            if (lecturer == null)
                return RedirectToAction("LecturerDashboard");

            ViewBag.Lecturer = lecturer;
            return View();
        }

        // POST: Submit Claim
        [HttpPost]
        public IActionResult ClaimForm(double hoursWorked, double hourlyRate, string notes, IFormFile supportingDocs)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null || HttpContext.Session.GetString("Role") != "Lecturer")
                return RedirectToAction("Login", "Account");

            var lecturer = _context.Lecturers.FirstOrDefault(l => l.UserID == userId);
            if (lecturer == null)
            {
                ViewBag.Error = "Lecturer record not found. Please contact admin.";
                return RedirectToAction("Login", "Account");
            }

            string docPath = null;
            if (supportingDocs != null && supportingDocs.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}_{supportingDocs.FileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    supportingDocs.CopyTo(stream);
                }
                docPath = $"/uploads/{fileName}";
            }

            var totalAmount = hoursWorked * hourlyRate; // auto calculate

            var claim = new Claim
            {
                LecturerID = lecturer.LecturerID, // FK is safe now
                HoursWorked = hoursWorked,
                Amount = totalAmount,
                SupportingDocumentPath = docPath,
                Status = "Pending",
                Description = notes,
                CoordinatorReview = "Pending",
                ManagerReview = "Pending",
                DateSubmitted = DateTime.Now
            };

            _context.Claims.Add(claim);
            _context.SaveChanges();

            return RedirectToAction("LecturerDashboard");
        }
    }
}
