using CollegeManagement.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeManagement.Web.Controllers;

[Authorize(Roles = "Faculty")]
public class FacultyPortalController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public FacultyPortalController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> MyCourses()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var items = await _context.FacultyCourseAssignments
            .Include(x => x.Course)
            .ThenInclude(x => x.Branch)
            .Include(x => x.FacultyProfile)
            .Where(x => x.FacultyProfile != null && x.FacultyProfile.IdentityUserId == user.Id)
            .ToListAsync();

        return View(items);
    }

    public async Task<IActionResult> MyStudents()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var courseIds = await _context.FacultyCourseAssignments
            .Where(x => x.FacultyProfile != null && x.FacultyProfile.IdentityUserId == user.Id)
            .Select(x => x.CourseId)
            .ToListAsync();

        var items = await _context.CourseEnrolments
            .Include(x => x.StudentProfile)
            .Include(x => x.Course)
            .ThenInclude(x => x.Branch)
            .Where(x => courseIds.Contains(x.CourseId))
            .ToListAsync();

        return View(items);
    }
}