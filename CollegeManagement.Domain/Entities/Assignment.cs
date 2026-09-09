using System.ComponentModel.DataAnnotations;

namespace CollegeManagement.Domain.Entities;

public class Assignment
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    [Required]
    [StringLength(150)]
    public string Title { get; set; } = string.Empty;

    public decimal MaxScore { get; set; }

    public DateTime DueDate { get; set; }

    public Course? Course { get; set; }

    public ICollection<AssignmentResult> AssignmentResults { get; set; } = new List<AssignmentResult>();
}
