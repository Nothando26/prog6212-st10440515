using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // <-- Needed for Include
using prog6212_st10440515_poe.Data;
using prog6212_st10440515_poe.Models;

namespace prog6212_st10440515_poe.Controllers
{
    public class ProgrammeCoordinatorController : Controller
    {
        private readonly AppDbContext _context;

        public ProgrammeCoordinatorController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Coordinator dashboard
        public IActionResult Coordinator()
        {
            var coordinatorId = HttpContext.Session.GetInt32("UserID");
            if (coordinatorId == null || HttpContext.Session.GetString("Role") != "Coordinator")
                return RedirectToAction("Login", "Account");

            // Get all claims to review and include Lecturer to avoid null references
            var claims = _context.Claims
                .Include(c => c.Lecturer) // <-- Eager load Lecturer
                .OrderByDescending(c => c.DateSubmitted)
                .ToList();

            return View(claims);
        }

        // POST: Update coordinator review
        [HttpPost]
        public IActionResult UpdateReview(int claimId, string actionType)
        {
            var claim = _context.Claims
                .Include(c => c.Lecturer) // Optional: include Lecturer if needed in processing
                .FirstOrDefault(c => c.ClaimID == claimId);

            if (claim == null)
                return RedirectToAction("Coordinator");

            switch (actionType)
            {
                case "Accept":
                    claim.CoordinatorReview = "Accepted";
                    break;
                case "Reject":
                    claim.CoordinatorReview = "Rejected";
                    break;
                case "Verify":
                    claim.CoordinatorReview = "Further Verification";
                    break;
            }

            _context.SaveChanges();
            return RedirectToAction("Coordinator");
        }
    }
}
