using FoodSafety.Domain.Enums;
using FoodSafety.MVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FoodSafety.MVC.Controllers
{
    [Authorize(Roles = "Admin,Inspector,Viewer")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? town, RiskRating? riskRating)
        {
            Log.Information("Dashboard opened by {User}. Filters: Town={Town}, RiskRating={RiskRating}",
                User?.Identity?.Name,
                town ?? "All",
                riskRating?.ToString() ?? "All");

            var premisesQuery = _context.Premises.AsQueryable();

            if (!string.IsNullOrWhiteSpace(town))
            {
                premisesQuery = premisesQuery.Where(p => p.Town == town);
            }

            if (riskRating.HasValue)
            {
                premisesQuery = premisesQuery.Where(p => p.RiskRating == riskRating.Value);
            }

            var premisesIds = await premisesQuery.Select(p => p.Id).ToListAsync();

            var startOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            var inspectionsThisMonth = await _context.Inspections
                .Where(i => i.InspectionDate >= startOfMonth && premisesIds.Contains(i.PremisesId))
                .CountAsync();

            var failedInspectionsThisMonth = await _context.Inspections
                .Where(i => i.InspectionDate >= startOfMonth
                            && i.Outcome == InspectionOutcome.Fail
                            && premisesIds.Contains(i.PremisesId))
                .CountAsync();

            var overdueOpenFollowUps = await _context.FollowUps
                .Include(f => f.Inspection)
                .ThenInclude(i => i.Premises)
                .Where(f => f.Status != FollowUpStatus.Closed
                            && f.DueDate < DateTime.Today
                            && premisesIds.Contains(f.Inspection!.PremisesId))
                .CountAsync();

            var recentInspections = await _context.Inspections
                .Include(i => i.Premises)
                .Where(i => premisesIds.Contains(i.PremisesId))
                .OrderByDescending(i => i.InspectionDate)
                .Take(10)
                .ToListAsync();

            ViewBag.Towns = new SelectList(
                await _context.Premises
                    .Select(p => p.Town)
                    .Distinct()
                    .OrderBy(t => t)
                    .ToListAsync());

            ViewBag.SelectedTown = town;
            ViewBag.SelectedRiskRating = riskRating;

            Log.Information("Dashboard metrics loaded. InspectionsThisMonth={InspectionsThisMonth}, FailedInspectionsThisMonth={FailedInspectionsThisMonth}, OverdueOpenFollowUps={OverdueOpenFollowUps}",
                inspectionsThisMonth,
                failedInspectionsThisMonth,
                overdueOpenFollowUps);

            var vm = new FoodSafety.MVC.Models.DashboardViewModel
            {
                InspectionsThisMonth = inspectionsThisMonth,
                FailedInspectionsThisMonth = failedInspectionsThisMonth,
                OverdueOpenFollowUps = overdueOpenFollowUps,
                RecentInspections = recentInspections
            };

            return View(vm);
        }
    }
}