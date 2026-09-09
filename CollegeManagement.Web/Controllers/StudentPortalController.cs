using CollegeManagement.Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CollegeManagement.Web.Controllers;

[Authorize(Roles = "Student")]
public class StudentPortalController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public StudentPortalController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> MyProfile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var profile = await _context.StudentProfiles
            .FirstOrDefaultAsync(x => x.IdentityUserId == user.Id);

        if (profile == null) return NotFound();

        return View(profile);
    }

    public async Task<IActionResult> MyEnrolments()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var items = await _context.CourseEnrolments
            .Include(x => x.Course)
            .ThenInclude(x => x.Branch)
            .Include(x => x.StudentProfile)
            .Where(x => x.StudentProfile != null && x.StudentProfile.IdentityUserId == user.Id)
            .ToListAsync();

        return View(items);
    }

    public async Task<IActionResult> MyExams()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var items = await _context.ExamResults
            .Include(x => x.Exam)
            .ThenInclude(x => x.Course)
            .Include(x => x.StudentProfile)
            .Where(x => x.StudentProfile != null
                     && x.StudentProfile.IdentityUserId == user.Id
                     && x.Exam != null
                     && x.Exam.ResultsReleased)
            .ToListAsync();

        return View(items);
    }
}