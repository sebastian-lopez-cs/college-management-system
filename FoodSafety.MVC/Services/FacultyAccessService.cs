using FoodSafety.MVC.Data;
using Microsoft.EntityFrameworkCore;

namespace FoodSafety.MVC.Services;

public class FacultyAccessService
{
    private readonly ApplicationDbContext _context;

    public FacultyAccessService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> FacultyOwnsCourseAsync(int facultyProfileId, int courseId)
    {
        return await _context.FacultyCourseAssignments
            .AnyAsync(x => x.FacultyProfileId == facultyProfileId && x.CourseId == courseId);
    }

    public async Task<List<int>> GetFacultyCourseIdsAsync(int facultyProfileId)
    {
        return await _context.FacultyCourseAssignments
            .Where(x => x.FacultyProfileId == facultyProfileId)
            .Select(x => x.CourseId)
            .ToListAsync();
    }
}
