using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodSafety.Domain.Entities;
using FoodSafety.MVC.Data;

namespace FoodSafety.MVC.Controllers
{
    public class FacultyProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FacultyProfilesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: FacultyProfiles
        public async Task<IActionResult> Index()
        {
            return View(await _context.FacultyProfiles.ToListAsync());
        }

        // GET: FacultyProfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facultyProfile = await _context.FacultyProfiles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (facultyProfile == null)
            {
                return NotFound();
            }

            return View(facultyProfile);
        }

        // GET: FacultyProfiles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FacultyProfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdentityUserId,Name,Email,Phone")] FacultyProfile facultyProfile)
        {
            if (ModelState.IsValid)
            {
                _context.Add(facultyProfile);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(facultyProfile);
        }

        // GET: FacultyProfiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facultyProfile = await _context.FacultyProfiles.FindAsync(id);
            if (facultyProfile == null)
            {
                return NotFound();
            }
            return View(facultyProfile);
        }

        // POST: FacultyProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdentityUserId,Name,Email,Phone")] FacultyProfile facultyProfile)
        {
            if (id != facultyProfile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facultyProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacultyProfileExists(facultyProfile.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(facultyProfile);
        }

        // GET: FacultyProfiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facultyProfile = await _context.FacultyProfiles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (facultyProfile == null)
            {
                return NotFound();
            }

            return View(facultyProfile);
        }

        // POST: FacultyProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var facultyProfile = await _context.FacultyProfiles.FindAsync(id);
            if (facultyProfile != null)
            {
                _context.FacultyProfiles.Remove(facultyProfile);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacultyProfileExists(int id)
        {
            return _context.FacultyProfiles.Any(e => e.Id == id);
        }
    }
}
