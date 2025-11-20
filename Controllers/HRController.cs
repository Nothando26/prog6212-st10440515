using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prog6212_st10440515_poe.Data;
using prog6212_st10440515_poe.Models;
using prog6212_st10440515_poe.Models.ViewModels;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace prog6212_st10440515_poe.Controllers
{
    [Authorize(Roles = "HR")]
    public class HRController : Controller
    {
        private readonly AppDbContext _db;

        public HRController(AppDbContext db)
        {
            _db = db;
        }

        // Show all users
        public IActionResult Index()
        {
            var users = _db.Users.ToList();
            return View(users);
        }

        // Create user form
        public IActionResult Create()
        {
            ViewBag.Roles = new[] { "Lecturer", "Coordinator", "Manager", "HR" };
            return View();
        }

        // Create user (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = new[] { "Lecturer", "Coordinator", "Manager", "HR" };
                return View(model);
            }

            // Prevent duplicate emails
            if (_db.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("", "Email already exists.");
                ViewBag.Roles = new[] { "Lecturer", "Coordinator", "Manager", "HR" };
                return View(model);
            }

            // Create user (WITH HASHED PASSWORD)
            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                PasswordHash = HashPassword(model.Password),
                Role = model.Role
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            // Auto-create Lecturer record
            if (model.Role == "Lecturer")
            {
                var nameParts = model.FullName.Split(' ');
                var first = nameParts[0];
                var last = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : "";

                var lecturer = new Lecturer
                {
                    UserID = user.UserID,
                    Name = first,
                    Surname = last,
                    Email = user.Email,
                    HourlyRate = 0
                };

                _db.Lecturers.Add(lecturer);
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        // Edit user
        public IActionResult Edit(int id)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserID == id);
            if (user == null) return NotFound();

            var model = new EditUserViewModel
            {
                UserID = user.UserID,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };

            ViewBag.Roles = new[] { "Lecturer", "Coordinator", "Manager", "HR" };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = new[] { "Lecturer", "Coordinator", "Manager", "HR" };
                return View(model);
            }

            var user = _db.Users.FirstOrDefault(u => u.UserID == model.UserID);
            if (user == null) return NotFound();

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.Role = model.Role;

            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // Reports
        public IActionResult Reports()
        {
            var report = _db.Lecturers
                .Include(l => l.Claims)
                .Select(l => new ClaimReportViewModel
                {
                    LecturerName = l.Name + " " + l.Surname,
                    TotalClaims = l.Claims.Count(),
                    TotalHours = l.Claims.Sum(c => c.HoursWorked),
                    TotalAmount = l.Claims.Sum(c => c.Amount)
                }).ToList();

            return View(report);
        }

        // Helper: Hash password
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}

