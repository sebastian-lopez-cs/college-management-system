namespace FoodSafety.Domain.Entities;

public class ExamResult
{
    public int Id { get; set; }

    public int ExamId { get; set; }

    public int StudentProfileId { get; set; }

    public decimal Score { get; set; }

    public string? Grade { get; set; }

    public Exam? Exam { get; set; }

    public StudentProfile? StudentProfile { get; set; }
}