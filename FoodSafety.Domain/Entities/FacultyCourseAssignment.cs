namespace FoodSafety.Domain.Entities;

public class FacultyCourseAssignment
{
    public int Id { get; set; }

    public int FacultyProfileId { get; set; }

    public int CourseId { get; set; }

    public FacultyProfile? FacultyProfile { get; set; }

    public Course? Course { get; set; }
}