using FoodSafety.Domain.Entities;
using FoodSafety.Domain.Enums;
using Xunit;

namespace FoodSafety.Tests
{
    public class InspectionTests
    {
        [Fact]
        public void Inspection_ShouldStoreValuesCorrectly()
        {
            var inspection = new Inspection
            {
                PremisesId = 1,
                InspectionDate = new DateTime(2026, 3, 26),
                InspectorName = "Sarah O'Brien",
                HygieneScore = 82,
                Outcome = InspectionOutcome.Pass,
                Notes = "Good hygiene standards."
            };

            Assert.Equal(1, inspection.PremisesId);
            Assert.Equal(new DateTime(2026, 3, 26), inspection.InspectionDate);
            Assert.Equal("Sarah O'Brien", inspection.InspectorName);
            Assert.Equal(82, inspection.HygieneScore);
            Assert.Equal(InspectionOutcome.Pass, inspection.Outcome);
            Assert.Equal("Good hygiene standards.", inspection.Notes);
        }
    }
}