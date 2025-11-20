using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prog6212_st10440515_poe.Data;
using prog6212_st10440515_poe.Models;
using System.Linq;

namespace prog6212_st10440515_poe.Controllers
{
    public class ProgrammeCoordinatorController : Controller
    {
        private readonly AppDbContext _context;

        public ProgrammeCoordinatorController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult CoordinatorDashboard()
        {
            var roleClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (roleClaim != "Coordinator") return RedirectToAction("Login", "Account");

            var claims = _context.Claims
                .Include(c => c.Lecturer)
                .OrderByDescending(c => c.DateSubmitted)
                .ToList();

            return View(claims);
        }

        [HttpPost]
        public IActionResult UpdateReview(int claimId, string actionType)
        {
            var roleClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            if (roleClaim != "Coordinator") return RedirectToAction("Login", "Account");

            var claim = _context.Claims.FirstOrDefault(c => c.ClaimID == claimId);
            if (claim == null)
            {
                TempData["Error"] = "Claim not found.";
                return RedirectToAction("CoordinatorDashboard");
            }

            if (!new[] { "Accept", "Reject", "Verify" }.Contains(actionType))
            {
                TempData["Error"] = "Invalid action.";
                return RedirectToAction("CoordinatorDashboard");
            }

            claim.CoordinatorReview = actionType switch
            {
                "Accept" => "Accepted",
                "Reject" => "Rejected",
                "Verify" => "Further Verification",
                _ => claim.CoordinatorReview
            };

            _context.SaveChanges();
            TempData["Success"] = $"Claim {actionType}ed successfully.";
            return RedirectToAction("CoordinatorDashboard");
        }
    }
}

