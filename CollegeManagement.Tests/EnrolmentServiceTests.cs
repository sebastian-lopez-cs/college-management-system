using CollegeManagement.Domain.Entities;
using CollegeManagement.Web.Data;
using CollegeManagement.Web.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CollegeManagement.Tests.Services;

public class EnrolmentServiceTests
{
    private ApplicationDbContext BuildContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task CanEnrolAsync_ReturnsFalse_WhenDuplicateExists()
    {
        using var context = BuildContext(nameof(CanEnrolAsync_ReturnsFalse_WhenDuplicateExists));

        context.CourseEnrolments.Add(new CourseEnrolment
        {
            StudentProfileId = 1,
            CourseId = 1,
            EnrolDate = DateTime.Today,
            Status = "Active"
        });

        context.SaveChanges();

        var service = new EnrolmentService(context);

        var result = await service.CanEnrolAsync(1, 1);

        Assert.False(result);
    }

    [Fact]
    public async Task CanEnrolAsync_ReturnsTrue_WhenNoDuplicateExists()
    {
        using var context = BuildContext(nameof(CanEnrolAsync_ReturnsTrue_WhenNoDuplicateExists));
        var service = new EnrolmentService(context);

        var result = await service.CanEnrolAsync(1, 1);

        Assert.True(result);
    }
}