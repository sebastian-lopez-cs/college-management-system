using CollegeManagement.Web.Services;
using Xunit;

namespace CollegeManagement.Tests.Services;

public class ResultVisibilityServiceTests
{
    private readonly ResultVisibilityService _service = new();

    [Fact]
    public void CanStudentSeeExamResult_ReturnsTrue_WhenReleased()
    {
        var result = _service.CanStudentSeeExamResult(true);

        Assert.True(result);
    }

    [Fact]
    public void CanStudentSeeExamResult_ReturnsFalse_WhenNotReleased()
    {
        var result = _service.CanStudentSeeExamResult(false);

        Assert.False(result);
    }

    [Fact]
    public void IsValidScore_ReturnsTrue_WhenScoreWithinRange()
    {
        var result = _service.IsValidScore(78, 100);

        Assert.True(result);
    }

    [Fact]
    public void IsValidScore_ReturnsFalse_WhenScoreAboveMax()
    {
        var result = _service.IsValidScore(120, 100);

        Assert.False(result);
    }

    [Fact]
    public void IsValidScore_ReturnsFalse_WhenScoreBelowZero()
    {
        var result = _service.IsValidScore(-1, 100);

        Assert.False(result);
    }

    [Fact]
    public void IsValidScore_ReturnsTrue_WhenScoreIsZero()
    {
        var result = _service.IsValidScore(0, 100);

        Assert.True(result);
    }

    [Fact]
    public void IsValidScore_ReturnsTrue_WhenScoreEqualsMaximum()
    {
        var result = _service.IsValidScore(100, 100);

        Assert.True(result);
    }
}
