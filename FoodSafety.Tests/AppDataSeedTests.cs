using FoodSafety.MVC.Data;
using FoodSafety.MVC.Seed;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FoodSafety.Tests
{
    public class AppDataSeedTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task SeedAppDataAsync_ShouldInsertPremisesInspectionsAndFollowUps()
        {
            using var context = GetDbContext();

            await AppDataSeed.SeedAppDataAsync(context);

            Assert.True(context.Premises.Count() >= 3);
            Assert.True(context.Inspections.Count() >= 1);
            Assert.True(context.FollowUps.Count() >= 1);
        }
    }
}