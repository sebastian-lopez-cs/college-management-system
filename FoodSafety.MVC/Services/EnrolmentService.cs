using FoodSafety.MVC.Data;
using Microsoft.EntityFrameworkCore;

namespace FoodSafety.MVC.Services;

public class EnrolmentService
{
    private readonly ApplicationDbContext _context;

    public EnrolmentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CanEnrolAsync(int studentProfileId, int courseId)
    {
        return !await _context.CourseEnrolments
            .AnyAsync(x => x.StudentProfileId == studentProfileId && x.CourseId == courseId);
    }
}
