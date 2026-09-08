using CollegeManagement.Domain.Entities;
using CollegeManagement.Domain.Enums;
using CollegeManagement.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CollegeManagement.Web.Controllers
{
    [Authorize]
    public class FollowUpsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FollowUpsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Inspector,Viewer")]
        public async Task<IActionResult> Index()
        {
            Log.Information("Follow-up list viewed by {User}.", User?.Identity?.Name);

            var followUps = _context.FollowUps
                .Include(f => f.Inspection)
                .ThenInclude(i => i.Premises)
                .OrderBy(f => f.DueDate);

            return View(await followUps.ToListAsync());
        }

        [Authorize(Roles = "Admin,Inspector,Viewer")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                Log.Warning("Follow-up details requested with null id by {User}.", User?.Identity?.Name);
                return NotFound();
            }

            var followUp = await _context.FollowUps
                .Include(f => f.Inspection)
                .ThenInclude(i => i.Premises)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (followUp == null)
            {
                Log.Warning("Follow-up details not found. Id={FollowUpId}, User={User}", id, User?.Identity?.Name);
                return NotFound();
            }

            Log.Information("Follow-up details viewed. Id={FollowUpId}, User={User}", id, User?.Identity?.Name);
            return View(followUp);
        }

        [Authorize(Roles = "Admin,Inspector")]
        public IActionResult Create()
        {
            Log.Information("Follow-up create page opened by {User}.", User?.Identity?.Name);

            LoadInspectionDropDown();
            return View(new FollowUp
            {
                DueDate = DateTime.Today.AddDays(14),
                Status = FollowUpStatus.Open
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Inspector")]
        public async Task<IActionResult> Create(FollowUp followUp)
        {
            if (followUp.CompletedDate.HasValue && followUp.CompletedDate.Value < followUp.DueDate.AddYears(-5))
            {
                ModelState.AddModelError("CompletedDate", "Completed date is not valid.");
            }

            if (ModelState.IsValid)
            {
                _context.FollowUps.Add(followUp);
                await _context.SaveChangesAsync();

                Log.Information("Follow-up created. Id={FollowUpId}, InspectionId={InspectionId}, DueDate={DueDate}, Status={Status}, User={User}",
                    followUp.Id, followUp.InspectionId, followUp.DueDate, followUp.Status, User?.Identity?.Name);

                return RedirectToAction(nameof(Index));
            }

            Log.Warning("Follow-up create failed validation. InspectionId={InspectionId}, User={User}",
                followUp.InspectionId, User?.Identity?.Name);

            LoadInspectionDropDown();
            return View(followUp);
        }

        [Authorize(Roles = "Admin,Inspector")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                Log.Warning("Follow-up edit requested with null id by {User}.", User?.Identity?.Name);
                return NotFound();
            }

            var followUp = await _context.FollowUps.FindAsync(id);

            if (followUp == null)
            {
                Log.Warning("Follow-up edit target not found. Id={FollowUpId}, User={User}", id, User?.Identity?.Name);
                return NotFound();
            }

            Log.Information("Follow-up edit page opened. Id={FollowUpId}, User={User}", id, User?.Identity?.Name);

            LoadInspectionDropDown();
            return View(followUp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Inspector")]
        public async Task<IActionResult> Edit(int id, FollowUp followUp)
        {
            if (id != followUp.Id)
            {
                Log.Warning("Follow-up edit id mismatch. RouteId={RouteId}, ModelId={ModelId}, User={User}",
                    id, followUp.Id, User?.Identity?.Name);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(followUp);
                    await _context.SaveChangesAsync();

                    Log.Information("Follow-up updated. Id={FollowUpId}, InspectionId={InspectionId}, Status={Status}, User={User}",
                        followUp.Id, followUp.InspectionId, followUp.Status, User?.Identity?.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.FollowUps.Any(e => e.Id == followUp.Id))
                    {
                        Log.Warning("Follow-up update failed because record no longer exists. Id={FollowUpId}, User={User}",
                            followUp.Id, User?.Identity?.Name);
                        return NotFound();
                    }

                    Log.Error("Follow-up update concurrency exception. Id={FollowUpId}, User={User}",
                        followUp.Id, User?.Identity?.Name);
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            Log.Warning("Follow-up edit failed validation. Id={FollowUpId}, User={User}",
                followUp.Id, User?.Identity?.Name);

            LoadInspectionDropDown();
            return View(followUp);
        }

        [Authorize(Roles = "Admin,Inspector")]
        public async Task<IActionResult> Close(int? id)
        {
            if (id == null)
            {
                Log.Warning("Follow-up close requested with null id by {User}.", User?.Identity?.Name);
                return NotFound();
            }

            var followUp = await _context.FollowUps
                .Include(f => f.Inspection)
                .ThenInclude(i => i.Premises)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (followUp == null)
            {
                Log.Warning("Follow-up close target not found. Id={FollowUpId}, User={User}", id, User?.Identity?.Name);
                return NotFound();
            }

            Log.Information("Follow-up close page opened. Id={FollowUpId}, User={User}", id, User?.Identity?.Name);
            return View(followUp);
        }

        [HttpPost, ActionName("Close")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Inspector")]
        public async Task<IActionResult> CloseConfirmed(int id)
        {
            var followUp = await _context.FollowUps.FindAsync(id);

            if (followUp == null)
            {
                Log.Warning("Follow-up close confirmed but record not found. Id={FollowUpId}, User={User}",
                    id, User?.Identity?.Name);
                return NotFound();
            }

            followUp.Status = FollowUpStatus.Closed;
            followUp.CompletedDate = DateTime.Today;

            await _context.SaveChangesAsync();

            Log.Information("Follow-up closed. Id={FollowUpId}, CompletedDate={CompletedDate}, User={User}",
                followUp.Id, followUp.CompletedDate, User?.Identity?.Name);

            return RedirectToAction(nameof(Index));
        }

        private void LoadInspectionDropDown()
        {
            var inspections = _context.Inspections
                .Include(i => i.Premises)
                .OrderByDescending(i => i.InspectionDate)
                .ToList()
                .Select(i => new
                {
                    i.Id,
                    Display = $"{i.Premises!.Name} - {i.InspectionDate:yyyy-MM-dd}"
                });

            ViewData["InspectionId"] = new SelectList(inspections, "Id", "Display");
        }
    }
}