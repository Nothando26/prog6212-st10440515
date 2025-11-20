using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using prog6212_st10440515_poe.Data;
using prog6212_st10440515_poe.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prog6212_st10440515_poe.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult SignUp() => RedirectToAction("Login");

        [HttpPost]
        public IActionResult Register() =>
            Unauthorized("Registration is disabled. Only HR can create accounts.");

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Email and password are required.";
                return View();
            }

            var user = _context.Users.FirstOrDefault(u =>
                u.Email.Trim().ToLower() == email.Trim().ToLower()
                && u.PasswordHash == password);

            if (user == null)
            {
                ViewBag.Error = "Invalid email or password.";
                return View();
            }

            var role = NormalizeRole(user.Role);

            // ✔ FULLY QUALIFIED CLAIM TYPES — NO ERRORS POSSIBLE
            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.FullName),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, role)
            };

            var identity = new System.Security.Claims.ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new System.Security.Claims.ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = true }
            );

            return RedirectToDashboard(role);
        }

        private IActionResult RedirectToDashboard(string role)
        {
            return role switch
            {
                "Lecturer" => RedirectToAction("LecturerDashboard", "Lecturer"),
                "Coordinator" => RedirectToAction("CoordinatorDashboard", "ProgrammeCoordinator"),
                "Manager" => RedirectToAction("ManagerDashboard", "AcademicManager"),
                "HR" => RedirectToAction("Index", "HR"),
                "Customer" => RedirectToAction("CustomerDashboard", "Customer"),
                _ => RedirectToAction("Index", "Home")
            };
        }

        private string NormalizeRole(string role)
        {
            if (role == null) return "Unknown";

            var r = role.Trim().ToLower();

            if (r.Contains("lect")) return "Lecturer";
            if (r.Contains("coord")) return "Coordinator";
            if (r.Contains("manager")) return "Manager";
            if (r.Contains("hr")) return "HR";
            if (r.Contains("cust")) return "Customer";

            return role;
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
