using CollegeManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CollegeManagement.Domain.Entities
{
    public class Inspection
    {
        public int Id { get; set; }

        [Required]
        public int PremisesId { get; set; }

        public Premises? Premises { get; set; }

        [Required]
        public DateTime InspectionDate { get; set; }

        [Required]
        [StringLength(120)]
        public string InspectorName { get; set; } = string.Empty;

        [Required]
        [Range(0, 100)]
        public int HygieneScore { get; set; }

        [Required]
        public InspectionOutcome Outcome { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        public ICollection<FollowUp> FollowUps { get; set; } = new List<FollowUp>();
    }
}
