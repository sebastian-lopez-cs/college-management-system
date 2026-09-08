using CollegeManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CollegeManagement.Domain.Entities
{
    public class FollowUp
    {
        public int Id { get; set; }

        [Required]
        public int InspectionId { get; set; }

        public Inspection? Inspection { get; set; }

        [Required]
        [StringLength(200)]
        public string ActionRequired { get; set; } = string.Empty;

        [Required]
        public DateTime DueDate { get; set; }

        public DateTime? CompletedDate { get; set; }

        [Required]
        public FollowUpStatus Status { get; set; }
    }
}