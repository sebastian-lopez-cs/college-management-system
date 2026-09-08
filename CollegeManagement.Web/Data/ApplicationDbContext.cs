using CollegeManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CollegeManagement.Web.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // OLD PROJECT TABLES - KEEP THEM
    public DbSet<Premises> Premises { get; set; } = default!;
    public DbSet<Inspection> Inspections { get; set; } = default!;
    public DbSet<FollowUp> FollowUps { get; set; } = default!;

    // NEW ASSIGNMENT TABLES
    public DbSet<Branch> Branches { get; set; } = default!;
    public DbSet<Course> Courses { get; set; } = default!;
    public DbSet<StudentProfile> StudentProfiles { get; set; } = default!;
    public DbSet<FacultyProfile> FacultyProfiles { get; set; } = default!;
    public DbSet<FacultyCourseAssignment> FacultyCourseAssignments { get; set; } = default!;
    public DbSet<CourseEnrolment> CourseEnrolments { get; set; } = default!;
    public DbSet<AttendanceRecord> AttendanceRecords { get; set; } = default!;
    public DbSet<Assignment> Assignments { get; set; } = default!;
    public DbSet<AssignmentResult> AssignmentResults { get; set; } = default!;
    public DbSet<Exam> Exams { get; set; } = default!;
    public DbSet<ExamResult> ExamResults { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Course>()
            .HasOne(c => c.Branch)
            .WithMany(b => b.Courses)
            .HasForeignKey(c => c.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StudentProfile>()
            .HasIndex(s => s.IdentityUserId)
            .IsUnique();

        modelBuilder.Entity<StudentProfile>()
            .HasIndex(s => s.StudentNumber)
            .IsUnique();

        modelBuilder.Entity<FacultyProfile>()
            .HasIndex(f => f.IdentityUserId)
            .IsUnique();

        modelBuilder.Entity<FacultyCourseAssignment>()
            .HasOne(fca => fca.FacultyProfile)
            .WithMany(fp => fp.FacultyCourseAssignments)
            .HasForeignKey(fca => fca.FacultyProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FacultyCourseAssignment>()
            .HasOne(fca => fca.Course)
            .WithMany(c => c.FacultyCourseAssignments)
            .HasForeignKey(fca => fca.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FacultyCourseAssignment>()
            .HasIndex(fca => new { fca.FacultyProfileId, fca.CourseId })
            .IsUnique();

        modelBuilder.Entity<CourseEnrolment>()
            .HasOne(ce => ce.StudentProfile)
            .WithMany(s => s.CourseEnrolments)
            .HasForeignKey(ce => ce.StudentProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CourseEnrolment>()
            .HasOne(ce => ce.Course)
            .WithMany(c => c.CourseEnrolments)
            .HasForeignKey(ce => ce.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CourseEnrolment>()
            .HasIndex(ce => new { ce.StudentProfileId, ce.CourseId })
            .IsUnique();

        modelBuilder.Entity<AttendanceRecord>()
            .HasOne(ar => ar.CourseEnrolment)
            .WithMany(ce => ce.AttendanceRecords)
            .HasForeignKey(ar => ar.CourseEnrolmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Assignment>()
            .HasOne(a => a.Course)
            .WithMany(c => c.Assignments)
            .HasForeignKey(a => a.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AssignmentResult>()
            .HasOne(ar => ar.Assignment)
            .WithMany(a => a.AssignmentResults)
            .HasForeignKey(ar => ar.AssignmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AssignmentResult>()
            .HasOne(ar => ar.StudentProfile)
            .WithMany(s => s.AssignmentResults)
            .HasForeignKey(ar => ar.StudentProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AssignmentResult>()
            .HasIndex(ar => new { ar.AssignmentId, ar.StudentProfileId })
            .IsUnique();

        modelBuilder.Entity<Exam>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Exams)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ExamResult>()
            .HasOne(er => er.Exam)
            .WithMany(e => e.ExamResults)
            .HasForeignKey(er => er.ExamId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ExamResult>()
            .HasOne(er => er.StudentProfile)
            .WithMany(s => s.ExamResults)
            .HasForeignKey(er => er.StudentProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ExamResult>()
            .HasIndex(er => new { er.ExamId, er.StudentProfileId })
            .IsUnique();
    }
}