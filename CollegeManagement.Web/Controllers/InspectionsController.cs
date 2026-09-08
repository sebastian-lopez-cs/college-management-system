using CollegeManagement.Domain.Entities;
using CollegeManagement.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CollegeManagement.Web.Controllers
{
    [Authorize]
    public class InspectionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InspectionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Inspector,Viewer")]
        public async Task<IActionResult> Index()
        {
            Log.Information("Inspections list viewed by {User}.", User?.Identity?.Name);

            var inspections = _context.Inspections
                .Include(i => i.Premises)
                .OrderByDescending(i => i.InspectionDate);

            return View(await inspections.ToListAsync());
        }

        [Authorize(Roles = "Admin,Inspector,Viewer")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                Log.Warning("Inspection details requested with null id by {User}.", User?.Identity?.Name);
                return NotFound();
            }

            var inspection = await _context.Inspections
                .Include(i => i.Premises)
                .Include(i => i.FollowUps)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inspection == null)
            {
                Log.Warning("Inspection details not found. Id={InspectionId}, User={User}", id, User?.Identity?.Name);
                return NotFound();
            }

            Log.Information("Inspection details viewed. Id={InspectionId}, User={User}", id, User?.Identity?.Name);
            return View(inspection);
        }

        [Authorize(Roles = "Admin,Inspector")]
        public IActionResult Create()
        {
            Log.Information("Inspection create page opened by {User}.", User?.Identity?.Name);
            LoadPremisesDropDown();
            return View(new Inspection { InspectionDate = DateTime.Today });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Inspector")]
        public async Task<IActionResult> Create(Inspection inspection)
        {
            if (inspection.HygieneScore < 0 || inspection.HygieneScore > 100)
            {
                ModelState.AddModelError("HygieneScore", "Score must be between 0 and 100.");
            }

            if (ModelState.IsValid)
            {
                _context.Inspections.Add(inspection);
                await _context.SaveChangesAsync();

                Log.Information("Inspection created. Id={InspectionId}, PremisesId={PremisesId}, Outcome={Outcome}, Score={Score}, User={User}",
                    inspection.Id, inspection.PremisesId, inspection.Outcome, inspection.HygieneScore, User?.Identity?.Name);

                return RedirectToAction(nameof(Index));
            }

            Log.Warning("Inspection create failed validation. PremisesId={PremisesId}, User={User}",
                inspection.PremisesId, User?.Identity?.Name);

            LoadPremisesDropDown();
            return View(inspection);
        }

        [Authorize(Roles = "Admin,Inspector")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                Log.Warning("Inspection edit requested with null id by {User}.", User?.Identity?.Name);
                return NotFound();
            }

            var inspection = await _context.Inspections.FindAsync(id);

            if (inspection == null)
            {
                Log.Warning("Inspection edit target not found. Id={InspectionId}, User={User}", id, User?.Identity?.Name);
                return NotFound();
            }

            Log.Information("Inspection edit page opened. Id={InspectionId}, User={User}", id, User?.Identity?.Name);

            LoadPremisesDropDown();
            return View(inspection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Inspector")]
        public async Task<IActionResult> Edit(int id, Inspection inspection)
        {
            if (id != inspection.Id)
            {
                Log.Warning("Inspection edit id mismatch. RouteId={RouteId}, ModelId={ModelId}, User={User}",
                    id, inspection.Id, User?.Identity?.Name);
                return NotFound();
            }

            if (inspection.HygieneScore < 0 || inspection.HygieneScore > 100)
            {
                ModelState.AddModelError("HygieneScore", "Score must be between 0 and 100.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inspection);
                    await _context.SaveChangesAsync();

                    Log.Information("Inspection updated. Id={InspectionId}, PremisesId={PremisesId}, Outcome={Outcome}, Score={Score}, User={User}",
                        inspection.Id, inspection.PremisesId, inspection.Outcome, inspection.HygieneScore, User?.Identity?.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Inspections.Any(e => e.Id == inspection.Id))
                    {
                        Log.Warning("Inspection update failed because record no longer exists. Id={InspectionId}, User={User}",
                            inspection.Id, User?.Identity?.Name);
                        return NotFound();
                    }

                    Log.Error("Inspection update concurrency exception. Id={InspectionId}, User={User}",
                        inspection.Id, User?.Identity?.Name);
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            Log.Warning("Inspection edit failed validation. Id={InspectionId}, User={User}",
                inspection.Id, User?.Identity?.Name);

            LoadPremisesDropDown();
            return View(inspection);
        }

        [Authorize(Roles = "Admin,Inspector")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                Log.Warning("Inspection delete requested with null id by {User}.", User?.Identity?.Name);
                return NotFound();
            }

            var inspection = await _context.Inspections
                .Include(i => i.Premises)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inspection == null)
            {
                Log.Warning("Inspection delete target not found. Id={InspectionId}, User={User}", id, User?.Identity?.Name);
                return NotFound();
            }

            Log.Information("Inspection delete page opened. Id={InspectionId}, User={User}", id, User?.Identity?.Name);
            return View(inspection);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Inspector")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inspection = await _context.Inspections.FindAsync(id);

            if (inspection != null)
            {
                _context.Inspections.Remove(inspection);
                await _context.SaveChangesAsync();

                Log.Information("Inspection deleted. Id={InspectionId}, User={User}",
                    inspection.Id, User?.Identity?.Name);
            }
            else
            {
                Log.Warning("Inspection delete confirmed but record not found. Id={InspectionId}, User={User}",
                    id, User?.Identity?.Name);
            }

            return RedirectToAction(nameof(Index));
        }

        private void LoadPremisesDropDown()
        {
            ViewData["PremisesId"] = new SelectList(_context.Premises.OrderBy(p => p.Name), "Id", "Name");
        }
    }
}