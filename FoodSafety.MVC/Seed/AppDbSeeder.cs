using FoodSafety.Domain.Entities;
using FoodSafety.MVC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FoodSafety.MVC.Seed;

public static class AppDbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await context.Database.MigrateAsync();

        // Roles
        string[] roles =
        {
            AppRoles.Administrator,
            AppRoles.Faculty,
            AppRoles.Student
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Admin user
        var adminEmail = "admin@vgc.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Pass123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, AppRoles.Administrator);
            }
        }
        else if (!await userManager.IsInRoleAsync(adminUser, AppRoles.Administrator))
        {
            await userManager.AddToRoleAsync(adminUser, AppRoles.Administrator);
        }

        // Faculty user
        var facultyEmail = "faculty@vgc.com";
        var facultyUser = await userManager.FindByEmailAsync(facultyEmail);
        if (facultyUser == null)
        {
            facultyUser = new IdentityUser
            {
                UserName = facultyEmail,
                Email = facultyEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(facultyUser, "Pass123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(facultyUser, AppRoles.Faculty);
            }
        }
        else if (!await userManager.IsInRoleAsync(facultyUser, AppRoles.Faculty))
        {
            await userManager.AddToRoleAsync(facultyUser, AppRoles.Faculty);
        }

        if (!await context.FacultyProfiles.AnyAsync(x => x.IdentityUserId == facultyUser!.Id))
        {
            context.FacultyProfiles.Add(new FacultyProfile
            {
                IdentityUserId = facultyUser.Id,
                Name = "John Faculty",
                Email = facultyEmail,
                Phone = "0850000001"
            });
        }

        // Student 1
        var student1Email = "student1@vgc.com";
        var student1User = await userManager.FindByEmailAsync(student1Email);
        if (student1User == null)
        {
            student1User = new IdentityUser
            {
                UserName = student1Email,
                Email = student1Email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(student1User, "Pass123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(student1User, AppRoles.Student);
            }
        }
        else if (!await userManager.IsInRoleAsync(student1User, AppRoles.Student))
        {
            await userManager.AddToRoleAsync(student1User, AppRoles.Student);
        }

        if (!await context.StudentProfiles.AnyAsync(x => x.IdentityUserId == student1User!.Id))
        {
            context.StudentProfiles.Add(new StudentProfile
            {
                IdentityUserId = student1User.Id,
                Name = "Student One",
                Email = student1Email,
                Phone = "0850000002",
                Address = "Dublin",
                StudentNumber = "ST001",
                DOB = new DateTime(2002, 1, 1)
            });
        }

        // Student 2
        var student2Email = "student2@vgc.com";
        var student2User = await userManager.FindByEmailAsync(student2Email);
        if (student2User == null)
        {
            student2User = new IdentityUser
            {
                UserName = student2Email,
                Email = student2Email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(student2User, "Pass123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(student2User, AppRoles.Student);
            }
        }
        else if (!await userManager.IsInRoleAsync(student2User, AppRoles.Student))
        {
            await userManager.AddToRoleAsync(student2User, AppRoles.Student);
        }

        if (!await context.StudentProfiles.AnyAsync(x => x.IdentityUserId == student2User!.Id))
        {
            context.StudentProfiles.Add(new StudentProfile
            {
                IdentityUserId = student2User.Id,
                Name = "Student Two",
                Email = student2Email,
                Phone = "0850000003",
                Address = "Dublin",
                StudentNumber = "ST002",
                DOB = new DateTime(2003, 2, 2)
            });
        }

        await context.SaveChangesAsync();

        // Branches
        if (!await context.Branches.AnyAsync())
        {
            context.Branches.AddRange(
                new Branch
                {
                    Name = "Dublin City Branch",
                    Address = "15 Main Street, Dublin"
                },
                new Branch
                {
                    Name = "Cork Branch",
                    Address = "22 College Road, Cork"
                },
                new Branch
                {
                    Name = "Galway Branch",
                    Address = "8 Harbour Lane, Galway"
                }
            );

            await context.SaveChangesAsync();
        }

        var dublinBranch = await context.Branches.SingleAsync(x => x.Name == "Dublin City Branch");
        var corkBranch = await context.Branches.SingleAsync(x => x.Name == "Cork Branch");
        var galwayBranch = await context.Branches.SingleAsync(x => x.Name == "Galway Branch");

        // Courses
        if (!await context.Courses.AnyAsync())
        {
            context.Courses.AddRange(
                new Course
                {
                    Name = "Business Computing",
                    BranchId = dublinBranch.Id,
                    StartDate = new DateTime(2026, 9, 1),
                    EndDate = new DateTime(2027, 5, 31)
                },
                new Course
                {
                    Name = "Software Development",
                    BranchId = corkBranch.Id,
                    StartDate = new DateTime(2026, 9, 1),
                    EndDate = new DateTime(2027, 5, 31)
                },
                new Course
                {
                    Name = "Data Analytics",
                    BranchId = galwayBranch.Id,
                    StartDate = new DateTime(2026, 9, 1),
                    EndDate = new DateTime(2027, 5, 31)
                }
            );

            await context.SaveChangesAsync();
        }

        var businessCourse = await context.Courses.SingleAsync(x => x.Name == "Business Computing");
        var softwareCourse = await context.Courses.SingleAsync(x => x.Name == "Software Development");
        var dataCourse = await context.Courses.SingleAsync(x => x.Name == "Data Analytics");

        var facultyProfile = await context.FacultyProfiles.SingleAsync(x => x.IdentityUserId == facultyUser!.Id);
        var student1Profile = await context.StudentProfiles.SingleAsync(x => x.IdentityUserId == student1User!.Id);
        var student2Profile = await context.StudentProfiles.SingleAsync(x => x.IdentityUserId == student2User!.Id);

        // Faculty-course assignments
        if (!await context.FacultyCourseAssignments.AnyAsync())
        {
            context.FacultyCourseAssignments.AddRange(
                new FacultyCourseAssignment
                {
                    FacultyProfileId = facultyProfile.Id,
                    CourseId = businessCourse.Id
                },
                new FacultyCourseAssignment
                {
                    FacultyProfileId = facultyProfile.Id,
                    CourseId = softwareCourse.Id
                }
            );

            await context.SaveChangesAsync();
        }

        // Enrolments
        if (!await context.CourseEnrolments.AnyAsync())
        {
            context.CourseEnrolments.AddRange(
                new CourseEnrolment
                {
                    StudentProfileId = student1Profile.Id,
                    CourseId = businessCourse.Id,
                    EnrolDate = new DateTime(2026, 9, 5),
                    Status = "Active"
                },
                new CourseEnrolment
                {
                    StudentProfileId = student2Profile.Id,
                    CourseId = businessCourse.Id,
                    EnrolDate = new DateTime(2026, 9, 5),
                    Status = "Active"
                },
                new CourseEnrolment
                {
                    StudentProfileId = student2Profile.Id,
                    CourseId = softwareCourse.Id,
                    EnrolDate = new DateTime(2026, 9, 6),
                    Status = "Active"
                }
            );

            await context.SaveChangesAsync();
        }

        var enrolmentStudent1Business = await context.CourseEnrolments
            .SingleAsync(x => x.StudentProfileId == student1Profile.Id && x.CourseId == businessCourse.Id);

        var enrolmentStudent2Business = await context.CourseEnrolments
            .SingleAsync(x => x.StudentProfileId == student2Profile.Id && x.CourseId == businessCourse.Id);

        var enrolmentStudent2Software = await context.CourseEnrolments
            .SingleAsync(x => x.StudentProfileId == student2Profile.Id && x.CourseId == softwareCourse.Id);

        // Attendance
        if (!await context.AttendanceRecords.AnyAsync())
        {
            context.AttendanceRecords.AddRange(
                new AttendanceRecord
                {
                    CourseEnrolmentId = enrolmentStudent1Business.Id,
                    WeekNumber = 1,
                    Date = new DateTime(2026, 9, 10),
                    Present = true
                },
                new AttendanceRecord
                {
                    CourseEnrolmentId = enrolmentStudent1Business.Id,
                    WeekNumber = 2,
                    Date = new DateTime(2026, 9, 17),
                    Present = true
                },
                new AttendanceRecord
                {
                    CourseEnrolmentId = enrolmentStudent2Business.Id,
                    WeekNumber = 1,
                    Date = new DateTime(2026, 9, 10),
                    Present = true
                },
                new AttendanceRecord
                {
                    CourseEnrolmentId = enrolmentStudent2Business.Id,
                    WeekNumber = 2,
                    Date = new DateTime(2026, 9, 17),
                    Present = false
                },
                new AttendanceRecord
                {
                    CourseEnrolmentId = enrolmentStudent2Software.Id,
                    WeekNumber = 1,
                    Date = new DateTime(2026, 9, 11),
                    Present = true
                }
            );

            await context.SaveChangesAsync();
        }

        // Assignments
        if (!await context.Assignments.AnyAsync())
        {
            context.Assignments.AddRange(
                new Assignment
                {
                    CourseId = businessCourse.Id,
                    Title = "Business Report",
                    MaxScore = 100,
                    DueDate = new DateTime(2026, 10, 15)
                },
                new Assignment
                {
                    CourseId = softwareCourse.Id,
                    Title = "C# MVC Project",
                    MaxScore = 100,
                    DueDate = new DateTime(2026, 10, 20)
                }
            );

            await context.SaveChangesAsync();
        }

        var businessAssignment = await context.Assignments.SingleAsync(x => x.Title == "Business Report");
        var softwareAssignment = await context.Assignments.SingleAsync(x => x.Title == "C# MVC Project");

        // Assignment results
        if (!await context.AssignmentResults.AnyAsync())
        {
            context.AssignmentResults.AddRange(
                new AssignmentResult
                {
                    AssignmentId = businessAssignment.Id,
                    StudentProfileId = student1Profile.Id,
                    Score = 78,
                    Feedback = "Good work"
                },
                new AssignmentResult
                {
                    AssignmentId = businessAssignment.Id,
                    StudentProfileId = student2Profile.Id,
                    Score = 65,
                    Feedback = "Satisfactory"
                },
                new AssignmentResult
                {
                    AssignmentId = softwareAssignment.Id,
                    StudentProfileId = student2Profile.Id,
                    Score = 88,
                    Feedback = "Very strong implementation"
                }
            );

            await context.SaveChangesAsync();
        }

        // Exams
        if (!await context.Exams.AnyAsync())
        {
            context.Exams.AddRange(
                new Exam
                {
                    CourseId = businessCourse.Id,
                    Title = "Business Computing Final Exam",
                    Date = new DateTime(2026, 12, 10),
                    MaxScore = 100,
                    ResultsReleased = true
                },
                new Exam
                {
                    CourseId = softwareCourse.Id,
                    Title = "Software Development Final Exam",
                    Date = new DateTime(2026, 12, 12),
                    MaxScore = 100,
                    ResultsReleased = false
                },
                new Exam
                {
                    CourseId = dataCourse.Id,
                    Title = "Data Analytics Final Exam",
                    Date = new DateTime(2026, 12, 15),
                    MaxScore = 100,
                    ResultsReleased = true
                }
            );

            await context.SaveChangesAsync();
        }

        var businessExam = await context.Exams.SingleAsync(x => x.Title == "Business Computing Final Exam");
        var softwareExam = await context.Exams.SingleAsync(x => x.Title == "Software Development Final Exam");

        // Exam results
        if (!await context.ExamResults.AnyAsync())
        {
            context.ExamResults.AddRange(
                new ExamResult
                {
                    ExamId = businessExam.Id,
                    StudentProfileId = student1Profile.Id,
                    Score = 81,
                    Grade = "Merit"
                },
                new ExamResult
                {
                    ExamId = businessExam.Id,
                    StudentProfileId = student2Profile.Id,
                    Score = 69,
                    Grade = "Pass"
                },
                new ExamResult
                {
                    ExamId = softwareExam.Id,
                    StudentProfileId = student2Profile.Id,
                    Score = 74,
                    Grade = "Provisional"
                }
            );

            await context.SaveChangesAsync();
        }
    }
}