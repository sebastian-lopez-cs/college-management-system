using System.ComponentModel.DataAnnotations;

namespace CollegeManagement.Domain.Entities;

public class Exam
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    [Required]
    [StringLength(150)]
    public string Title { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public decimal MaxScore { get; set; }

    public bool ResultsReleased { get; set; }

    public Course? Course { get; set; }

    public ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();
}