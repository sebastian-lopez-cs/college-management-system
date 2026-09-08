namespace FoodSafety.MVC.Services;

public class ResultVisibilityService
{
    public bool CanStudentSeeExamResult(bool resultsReleased)
    {
        return resultsReleased;
    }

    public bool IsValidScore(decimal score, decimal maxScore)
    {
        return score >= 0 && score <= maxScore;
    }
}
