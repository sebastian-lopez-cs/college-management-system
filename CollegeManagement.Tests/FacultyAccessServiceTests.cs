using CollegeManagement.Domain.Entities;
using CollegeManagement.Web.Data;
using CollegeManagement.Web.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CollegeManagement.Tests.Services;

public class FacultyAccessServiceTests
{
    private ApplicationDbContext BuildContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task FacultyOwnsCourseAsync_ReturnsTrue_WhenFacultyAssignedToCourse()
    {
        using var context = BuildContext(nameof(FacultyOwnsCourseAsync_ReturnsTrue_WhenFacultyAssignedToCourse));

        context.FacultyCourseAssignments.Add(new FacultyCourseAssignment
        {
            FacultyProfileId = 1,
            CourseId = 10
        });

        context.SaveChanges();

        var service = new FacultyAccessService(context);

        var result = await service.FacultyOwnsCourseAsync(1, 10);

        Assert.True(result);
    }

    [Fact]
    public async Task FacultyOwnsCourseAsync_ReturnsFalse_WhenFacultyNotAssignedToCourse()
    {
        using var context = BuildContext(nameof(FacultyOwnsCourseAsync_ReturnsFalse_WhenFacultyNotAssignedToCourse));

        context.FacultyCourseAssignments.Add(new FacultyCourseAssignment
        {
            FacultyProfileId = 1,
            CourseId = 10
        });

        context.SaveChanges();

        var service = new FacultyAccessService(context);

        var result = await service.FacultyOwnsCourseAsync(1, 99);

        Assert.False(result);
    }

    [Fact]
    public async Task GetFacultyCourseIdsAsync_ReturnsOnlyFacultyCourses()
    {
        using var context = BuildContext(nameof(GetFacultyCourseIdsAsync_ReturnsOnlyFacultyCourses));

        context.FacultyCourseAssignments.AddRange(
            new FacultyCourseAssignment { FacultyProfileId = 1, CourseId = 10 },
            new FacultyCourseAssignment { FacultyProfileId = 1, CourseId = 20 },
            new FacultyCourseAssignment { FacultyProfileId = 2, CourseId = 30 }
        );

        context.SaveChanges();

        var service = new FacultyAccessService(context);

        var result = await service.GetFacultyCourseIdsAsync(1);

        Assert.Equal(2, result.Count);
        Assert.Contains(10, result);
        Assert.Contains(20, result);
        Assert.DoesNotContain(30, result);
    }
}