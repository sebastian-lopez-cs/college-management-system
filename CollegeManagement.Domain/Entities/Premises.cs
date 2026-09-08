using CollegeManagement.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CollegeManagement.Domain.Entities
{
    public class Premises
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string AddressLine1 { get; set; } = string.Empty;

        [Required]
        [StringLength(80)]
        public string Town { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Eircode { get; set; } = string.Empty;

        [Required]
        [StringLength(80)]
        public string BusinessType { get; set; } = string.Empty;

        [Required]
        public RiskRating RiskRating { get; set; }

        public ICollection<Inspection> Inspections { get; set; } = new List<Inspection>();
    }
}