using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prog6212_st10440515_poe.Data;

using prog6212_st10440515_poe.Models;

namespace prog6212_st10440515_poe.Controllers
{
    public class AcademicManagerController : Controller
    {
        private readonly AppDbContext _context;

        public AcademicManagerController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Dashboard
        public IActionResult Manager()
        {
            var managerId = HttpContext.Session.GetInt32("UserID");
            if (managerId == null || HttpContext.Session.GetString("Role") != "Manager")
                return RedirectToAction("Login", "Account");

            // Fetch all lecturers and their claims
            var claims = _context.Claims
                .OrderByDescending(c => c.DateSubmitted)
                .ToList();

            return View(claims);
        }

        // GET: Review page
        public IActionResult Review(int claimId)
        {
            var managerId = HttpContext.Session.GetInt32("UserID");
            if (managerId == null || HttpContext.Session.GetString("Role") != "Manager")
                return RedirectToAction("Login", "Account");

            var claim = _context.Claims.FirstOrDefault(c => c.ClaimID == claimId);
            if (claim == null)
                return RedirectToAction("Manager");

            return View(claim);
        }

        // POST: Update review
        [HttpPost]
        public IActionResult Review(int claimId, string actionType)
        {
            var claim = _context.Claims.FirstOrDefault(c => c.ClaimID == claimId);
            if (claim == null)
                return RedirectToAction("Manager");

            switch (actionType)
            {
                case "Accept":
                    claim.ManagerReview = "Accepted";
                    break;
                case "Reject":
                    claim.ManagerReview = "Rejected";
                    break;
                case "Verify":
                    claim.ManagerReview = "Further Verification";
                    break;
            }

            _context.SaveChanges();
            return RedirectToAction("Manager");
        }
    }
}
