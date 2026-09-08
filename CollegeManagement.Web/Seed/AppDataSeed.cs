using CollegeManagement.Domain.Entities;
using CollegeManagement.Domain.Enums;
using CollegeManagement.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace CollegeManagement.Web.Seed
{
    public static class AppDataSeed
    {
        public static async Task SeedAppDataAsync(ApplicationDbContext context)
        {
            await context.Database.MigrateAsync();

            if (context.Premises.Any())
                return;

            var premisesList = new List<Premises>
            {
                new Premises { Name = "Green Apple Cafe", AddressLine1 = "12 Main Street", Town = "Dublin", Eircode = "D01A1A1", BusinessType = "Cafe", RiskRating = RiskRating.Medium },
                new Premises { Name = "Riverbank Restaurant", AddressLine1 = "45 River Road", Town = "Dublin", Eircode = "D02B2B2", BusinessType = "Restaurant", RiskRating = RiskRating.High },
                new Premises { Name = "Sunny Bakery", AddressLine1 = "8 Market Lane", Town = "Cork", Eircode = "T12C3C3", BusinessType = "Bakery", RiskRating = RiskRating.Low },
                new Premises { Name = "Quick Bite", AddressLine1 = "99 Station Road", Town = "Galway", Eircode = "H91D4D4", BusinessType = "Takeaway", RiskRating = RiskRating.High },
                new Premises { Name = "Harbour Hotel Kitchen", AddressLine1 = "5 Dock Street", Town = "Limerick", Eircode = "V94E5E5", BusinessType = "Hotel Kitchen", RiskRating = RiskRating.Medium },
                new Premises { Name = "Fresh Farm Foods", AddressLine1 = "21 Green Park", Town = "Cork", Eircode = "T12F6F6", BusinessType = "Retail Food Shop", RiskRating = RiskRating.Medium },
                new Premises { Name = "City Deli", AddressLine1 = "61 O'Connell Street", Town = "Dublin", Eircode = "D03G7G7", BusinessType = "Deli", RiskRating = RiskRating.Low },
                new Premises { Name = "Bella Pizza", AddressLine1 = "14 Church Street", Town = "Galway", Eircode = "H91H8H8", BusinessType = "Restaurant", RiskRating = RiskRating.High },
                new Premises { Name = "Golden Spoon", AddressLine1 = "7 High Street", Town = "Limerick", Eircode = "V94I9I9", BusinessType = "Cafe", RiskRating = RiskRating.Low },
                new Premises { Name = "Ocean View Bar", AddressLine1 = "3 Coast Road", Town = "Cork", Eircode = "T12J1J1", BusinessType = "Bar Kitchen", RiskRating = RiskRating.Medium },
                new Premises { Name = "Campus Canteen", AddressLine1 = "1 College Avenue", Town = "Dublin", Eircode = "D04K2K2", BusinessType = "Canteen", RiskRating = RiskRating.High },
                new Premises { Name = "Healthy Bowls", AddressLine1 = "18 Elm Street", Town = "Galway", Eircode = "H91L3L3", BusinessType = "Takeaway", RiskRating = RiskRating.Medium }
            };

            context.Premises.AddRange(premisesList);
            await context.SaveChangesAsync();

            var premises = await context.Premises.ToListAsync();

            var inspections = new List<Inspection>
            {
                new Inspection { PremisesId = premises[0].Id, InspectionDate = DateTime.Today.AddDays(-5), InspectorName = "Sarah O'Brien", HygieneScore = 82, Outcome = InspectionOutcome.Pass, Notes = "Good overall hygiene standards." },
                new Inspection { PremisesId = premises[1].Id, InspectionDate = DateTime.Today.AddDays(-3), InspectorName = "Mark Kelly", HygieneScore = 48, Outcome = InspectionOutcome.Fail, Notes = "Temperature records incomplete." },
                new Inspection { PremisesId = premises[2].Id, InspectionDate = DateTime.Today.AddDays(-20), InspectorName = "Sarah O'Brien", HygieneScore = 91, Outcome = InspectionOutcome.Pass, Notes = "Very clean preparation area." },
                new Inspection { PremisesId = premises[3].Id, InspectionDate = DateTime.Today.AddDays(-10), InspectorName = "Lisa Murphy", HygieneScore = 39, Outcome = InspectionOutcome.Fail, Notes = "Cross contamination risk observed." },
                new Inspection { PremisesId = premises[4].Id, InspectionDate = DateTime.Today.AddDays(-15), InspectorName = "Mark Kelly", HygieneScore = 67, Outcome = InspectionOutcome.ConditionalPass, Notes = "Storage labels need improvement." },
                new Inspection { PremisesId = premises[5].Id, InspectionDate = DateTime.Today.AddDays(-7), InspectorName = "Lisa Murphy", HygieneScore = 74, Outcome = InspectionOutcome.Pass, Notes = "Minor record keeping issues only." },
                new Inspection { PremisesId = premises[6].Id, InspectionDate = DateTime.Today.AddDays(-30), InspectorName = "Sarah O'Brien", HygieneScore = 88, Outcome = InspectionOutcome.Pass, Notes = "All checks satisfactory." },
                new Inspection { PremisesId = premises[7].Id, InspectionDate = DateTime.Today.AddDays(-2), InspectorName = "Mark Kelly", HygieneScore = 44, Outcome = InspectionOutcome.Fail, Notes = "Cleaning schedule not followed." },
                new Inspection { PremisesId = premises[8].Id, InspectionDate = DateTime.Today.AddDays(-12), InspectorName = "Lisa Murphy", HygieneScore = 79, Outcome = InspectionOutcome.Pass, Notes = "Improved since last inspection." },
                new Inspection { PremisesId = premises[9].Id, InspectionDate = DateTime.Today.AddDays(-25), InspectorName = "Sarah O'Brien", HygieneScore = 63, Outcome = InspectionOutcome.ConditionalPass, Notes = "Bar fridge calibration required." },
                new Inspection { PremisesId = premises[10].Id, InspectionDate = DateTime.Today.AddDays(-1), InspectorName = "Mark Kelly", HygieneScore = 41, Outcome = InspectionOutcome.Fail, Notes = "Handwashing station poorly stocked." },
                new Inspection { PremisesId = premises[11].Id, InspectionDate = DateTime.Today.AddDays(-18), InspectorName = "Lisa Murphy", HygieneScore = 72, Outcome = InspectionOutcome.Pass, Notes = "Acceptable with small recommendations." }
            };

            context.Inspections.AddRange(inspections);
            await context.SaveChangesAsync();

            var savedInspections = await context.Inspections.Include(i => i.Premises).ToListAsync();

            var followUps = new List<FollowUp>
            {
                new FollowUp { InspectionId = savedInspections[1].Id, ActionRequired = "Complete temperature log daily", DueDate = DateTime.Today.AddDays(7), Status = FollowUpStatus.Open },
                new FollowUp { InspectionId = savedInspections[3].Id, ActionRequired = "Separate raw and cooked storage", DueDate = DateTime.Today.AddDays(-2), Status = FollowUpStatus.Open },
                new FollowUp { InspectionId = savedInspections[4].Id, ActionRequired = "Update all storage labels", DueDate = DateTime.Today.AddDays(5), Status = FollowUpStatus.InProgress },
                new FollowUp { InspectionId = savedInspections[7].Id, ActionRequired = "Implement cleaning checklist", DueDate = DateTime.Today.AddDays(-1), Status = FollowUpStatus.Open },
                new FollowUp { InspectionId = savedInspections[9].Id, ActionRequired = "Calibrate bar fridge", DueDate = DateTime.Today.AddDays(3), Status = FollowUpStatus.InProgress },
                new FollowUp { InspectionId = savedInspections[10].Id, ActionRequired = "Restock and monitor handwashing station", DueDate = DateTime.Today.AddDays(4), Status = FollowUpStatus.Open },
                new FollowUp { InspectionId = savedInspections[1].Id, ActionRequired = "Train staff on recording procedures", DueDate = DateTime.Today.AddDays(10), Status = FollowUpStatus.Open },
                new FollowUp { InspectionId = savedInspections[3].Id, ActionRequired = "Review food prep workflow", DueDate = DateTime.Today.AddDays(6), Status = FollowUpStatus.InProgress },
                new FollowUp { InspectionId = savedInspections[7].Id, ActionRequired = "Deep clean food prep area", DueDate = DateTime.Today.AddDays(2), Status = FollowUpStatus.Open },
                new FollowUp { InspectionId = savedInspections[10].Id, ActionRequired = "Supervisor recheck hygiene station", DueDate = DateTime.Today.AddDays(8), Status = FollowUpStatus.Open }
            };

            context.FollowUps.AddRange(followUps);
            await context.SaveChangesAsync();
        }
    }
}