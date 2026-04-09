namespace FoodSafety.Domain.Entities;

public class AttendanceRecord
{
    public int Id { get; set; }

    public int CourseEnrolmentId { get; set; }

    public int WeekNumber { get; set; }

    public DateTime Date { get; set; }

    public bool Present { get; set; }

    public CourseEnrolment? CourseEnrolment { get; set; }
}