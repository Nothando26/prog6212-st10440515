using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prog6212_st10440515_poe.Data;
using prog6212_st10440515_poe.Models;

namespace prog6212_st10440515_poe.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Sign Up
        public IActionResult SignUp()
        {
            return View();
        }

        // POST: Sign Up (auto login + redirect)
        [HttpPost]
        public IActionResult Register(string fullName, string email, string password, string role)
        {
            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(role))
            {
                ViewBag.Error = "All fields are required.";
                return View("SignUp");
            }

            // Normalize role value
            var normalizedRole = NormalizeRole(role);

            if (_context.Users.Any(u => u.Email == email))
            {
                ViewBag.Error = "Email already exists.";
                return View("SignUp");
            }

            var newUser = new User
            {
                FullName = fullName.Trim(),
                Email = email.Trim(),
                PasswordHash = HashPassword(password),
                Role = normalizedRole
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            // If the user signed up as a Lecturer, create a Lecturer record as well
            if (string.Equals(normalizedRole, "lecturer", StringComparison.OrdinalIgnoreCase))
            {
                // Attempt to split full name for Name / Surname. Best-effort.
                var names = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string first = names.Length > 0 ? names[0] : fullName.Trim();
                string last = names.Length > 1 ? string.Join(' ', names.Skip(1)) : "";

                // Only create if a Lecturer for this user doesn't already exist
                var existing = _context.Lecturers.FirstOrDefault(l => l.UserID == newUser.UserID);
                if (existing == null)
                {
                    var lecturer = new Lecturer
                    {
                        UserID = newUser.UserID,
                        Name = first,
                        Surname = last,
                        Email = newUser.Email,
                        HourlyRate = 0 // default, can be updated later
                    };
                    _context.Lecturers.Add(lecturer);
                    _context.SaveChanges();
                }
            }

            // Auto-login
            HttpContext.Session.SetInt32("UserID", newUser.UserID);
            HttpContext.Session.SetString("Role", newUser.Role);

            // Redirect based on role
            return RedirectToDashboard(newUser.Role);
        }

        // GET: Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Email and password are required.";
                return View();
            }

            var passwordHash = HashPassword(password);
            var user = _context.Users.FirstOrDefault(u => u.Email.Trim().ToLower() == email.Trim().ToLower()
                                                          && u.PasswordHash == passwordHash);

            if (user != null)
            {
                // Normalize role BEFORE saving to session
                var normalizedRole = NormalizeRole(user.Role);

                HttpContext.Session.SetInt32("UserID", user.UserID);
                HttpContext.Session.SetString("Role", normalizedRole);

                Console.WriteLine($"[DEBUG] Logged in as {user.Email}, role = '{normalizedRole}'");

                return RedirectToDashboard(normalizedRole); // Use normalized role consistently
            }

            // Invalid credentials
            ViewBag.Error = "Invalid email or password.";
            return View();
        }


        // Helper: map role to dashboard (case-insensitive, tolerant)
        private IActionResult RedirectToDashboard(string role)
        {
            switch (role)
            {
                case "Lecturer":
                    return RedirectToAction("LecturerDashboard", "Lecturer");
                case "Coordinator":
                    return RedirectToAction("Coordinator", "ProgrammeCoordinator");
                case "Manager":
                    return RedirectToAction("Manager", "AcademicManager");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }


        // Normalize role on input: lower-case trimmed 1-word canonical forms
        private string NormalizeRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                return role;

            var r = role.Trim().ToLowerInvariant();
            if (r.Contains("lect")) return "Lecturer";
            if (r.Contains("coordinator") || r.Contains("programme")) return "Coordinator";
            if (r.Contains("manager") || r.Contains("academic")) return "Manager";
            // default: preserve as-is with capitalization
            return char.ToUpper(r[0]) + r.Substring(1);
        }

        // Secure SHA256 hashing (demo)
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}

