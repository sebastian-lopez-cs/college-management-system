using CollegeManagement.Domain.Entities;
using CollegeManagement.Domain.Enums;
using Xunit;

namespace CollegeManagement.Tests
{
    public class PremisesTests
    {
        [Fact]
        public void Premises_ShouldStoreValuesCorrectly()
        {
            var premises = new Premises
            {
                Name = "Riverbank Restaurant",
                AddressLine1 = "45 River Road",
                Town = "Dublin",
                Eircode = "D02B2B2",
                BusinessType = "Restaurant",
                RiskRating = RiskRating.High
            };

            Assert.Equal("Riverbank Restaurant", premises.Name);
            Assert.Equal("45 River Road", premises.AddressLine1);
            Assert.Equal("Dublin", premises.Town);
            Assert.Equal("D02B2B2", premises.Eircode);
            Assert.Equal("Restaurant", premises.BusinessType);
            Assert.Equal(RiskRating.High, premises.RiskRating);
        }
    }
}