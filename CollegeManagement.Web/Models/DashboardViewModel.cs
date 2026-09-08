using CollegeManagement.Domain.Entities;

namespace CollegeManagement.Web.Models
{
    public class DashboardViewModel
    {
        public int InspectionsThisMonth { get; set; }
        public int FailedInspectionsThisMonth { get; set; }
        public int OverdueOpenFollowUps { get; set; }
        public List<Inspection> RecentInspections { get; set; } = new();
    }
}