using CollegeManagement.Domain.Entities;
using CollegeManagement.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CollegeManagement.Web.Controllers
{
    [Authorize]
    public class PremisesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PremisesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Inspector,Viewer")]
        public async Task<IActionResult> Index()
        {
            Log.Information("Premises list viewed by {User}.", User?.Identity?.Name);
            return View(await _context.Premises.ToListAsync());
        }

        [Authorize(Roles = "Admin,Inspector,Viewer")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                Log.Warning("Premises details requested with null id by {User}.", User?.Identity?.Name);
                return NotFound();
            }

            var premises = await _context.Premises.FirstOrDefaultAsync(p => p.Id == id);

            if (premises == null)
            {
                Log.Warning("Premises details not found for Id={PremisesId}. User={User}", id, User?.Identity?.Name);
                return NotFound();
            }

            Log.Information("Premises details viewed for Id={PremisesId} by {User}.", id, User?.Identity?.Name);
            return View(premises);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            Log.Information("Premises create page opened by {User}.", User?.Identity?.Name);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Premises premises)
        {
            if (ModelState.IsValid)
            {
                _context.Premises.Add(premises);
                await _context.SaveChangesAsync();

                Log.Information("Premises created. Id={PremisesId}, Name={Name}, User={User}",
                    premises.Id, premises.Name, User?.Identity?.Name);

                return RedirectToAction(nameof(Index));
            }

            Log.Warning("Premises create failed validation for Name={Name}. User={User}",
                premises.Name, User?.Identity?.Name);

            return View(premises);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                Log.Warning("Premises edit requested with null id by {User}.", User?.Identity?.Name);
                return NotFound();
            }

            var premises = await _context.Premises.FindAsync(id);

            if (premises == null)
            {
                Log.Warning("Premises edit target not found. Id={PremisesId}, User={User}", id, User?.Identity?.Name);
                return NotFound();
            }

            Log.Information("Premises edit page opened for Id={PremisesId} by {User}.", id, User?.Identity?.Name);
            return View(premises);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Premises premises)
        {
            if (id != premises.Id)
            {
                Log.Warning("Premises edit id mismatch. RouteId={RouteId}, ModelId={ModelId}, User={User}",
                    id, premises.Id, User?.Identity?.Name);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(premises);
                    await _context.SaveChangesAsync();

                    Log.Information("Premises updated. Id={PremisesId}, Name={Name}, User={User}",
                        premises.Id, premises.Name, User?.Identity?.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Premises.Any(e => e.Id == premises.Id))
                    {
                        Log.Warning("Premises update failed because record no longer exists. Id={PremisesId}, User={User}",
                            premises.Id, User?.Identity?.Name);
                        return NotFound();
                    }

                    Log.Error("Premises update concurrency exception. Id={PremisesId}, User={User}",
                        premises.Id, User?.Identity?.Name);
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            Log.Warning("Premises edit failed validation. Id={PremisesId}, User={User}",
                premises.Id, User?.Identity?.Name);

            return View(premises);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                Log.Warning("Premises delete requested with null id by {User}.", User?.Identity?.Name);
                return NotFound();
            }

            var premises = await _context.Premises.FirstOrDefaultAsync(p => p.Id == id);

            if (premises == null)
            {
                Log.Warning("Premises delete target not found. Id={PremisesId}, User={User}", id, User?.Identity?.Name);
                return NotFound();
            }

            Log.Information("Premises delete page opened for Id={PremisesId} by {User}.", id, User?.Identity?.Name);
            return View(premises);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var premises = await _context.Premises.FindAsync(id);

            if (premises != null)
            {
                _context.Premises.Remove(premises);
                await _context.SaveChangesAsync();

                Log.Information("Premises deleted. Id={PremisesId}, Name={Name}, User={User}",
                    premises.Id, premises.Name, User?.Identity?.Name);
            }
            else
            {
                Log.Warning("Premises delete confirmed but record not found. Id={PremisesId}, User={User}",
                    id, User?.Identity?.Name);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}