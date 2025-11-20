using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prog6212_st10440515_poe.Data;
using prog6212_st10440515_poe.Models;
using System.Linq;

namespace prog6212_st10440515_poe.Controllers
{
    public class AcademicManagerController : Controller
    {
        private readonly AppDbContext _context;

        public AcademicManagerController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Manager Dashboard
        public IActionResult ManagerDashboard()
        {
            var roleClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (roleClaim != "Manager")
                return RedirectToAction("Login", "Account");

            var claims = _context.Claims
                .Include(c => c.Lecturer)
                .OrderByDescending(c => c.DateSubmitted)
                .ToList();

            return View(claims);
        }

        // POST: Update Manager Review
        [HttpPost]
        public IActionResult UpdateReview(int claimId, string actionType)
        {
            var roleClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (roleClaim != "Manager") return RedirectToAction("Login", "Account");

            var claim = _context.Claims.FirstOrDefault(c => c.ClaimID == claimId);
            if (claim == null)
            {
                TempData["Error"] = "Claim not found.";
                return RedirectToAction("ManagerDashboard");
            }

            if (!new[] { "Accept", "Reject", "Verify" }.Contains(actionType))
            {
                TempData["Error"] = "Invalid action.";
                return RedirectToAction("ManagerDashboard");
            }

            claim.ManagerReview = actionType switch
            {
                "Accept" => "Accepted",
                "Reject" => "Rejected",
                "Verify" => "Further Verification",
                _ => claim.ManagerReview
            };

            _context.SaveChanges();
            TempData["Success"] = $"Claim {actionType}ed successfully.";
            return RedirectToAction("ManagerDashboard");
        }
    }
}

