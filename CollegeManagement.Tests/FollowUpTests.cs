using CollegeManagement.Domain.Entities;
using CollegeManagement.Domain.Enums;
using Xunit;

namespace CollegeManagement.Tests
{
    public class FollowUpTests
    {
        [Fact]
        public void NewFollowUp_ShouldStoreValuesCorrectly()
        {
            var followUp = new FollowUp
            {
                InspectionId = 2,
                ActionRequired = "Complete temperature log daily",
                DueDate = new DateTime(2026, 4, 5),
                Status = FollowUpStatus.Open
            };

            Assert.Equal(2, followUp.InspectionId);
            Assert.Equal("Complete temperature log daily", followUp.ActionRequired);
            Assert.Equal(new DateTime(2026, 4, 5), followUp.DueDate);
            Assert.Equal(FollowUpStatus.Open, followUp.Status);
            Assert.Null(followUp.CompletedDate);
        }

        [Fact]
        public void ClosingFollowUp_ShouldSetStatusClosedAndCompletedDate()
        {
            var followUp = new FollowUp
            {
                InspectionId = 2,
                ActionRequired = "Deep clean food prep area",
                DueDate = DateTime.Today.AddDays(2),
                Status = FollowUpStatus.Open
            };

            followUp.Status = FollowUpStatus.Closed;
            followUp.CompletedDate = DateTime.Today;

            Assert.Equal(FollowUpStatus.Closed, followUp.Status);
            Assert.Equal(DateTime.Today, followUp.CompletedDate);
        }
    }
}