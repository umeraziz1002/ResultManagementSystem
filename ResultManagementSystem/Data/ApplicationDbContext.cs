using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ResultManagementSystem.Models;

namespace ResultManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<AcademicProgram> AcademicPrograms { get; set; }
        public DbSet<Batch> Batches { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<CourseOffering> CourseOfferings { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<GradePolicy> GradePolicies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // MARK precision
            builder.Entity<Mark>(entity =>
            {
                entity.Property(e => e.Quiz).HasPrecision(5, 2);
                entity.Property(e => e.Assignment).HasPrecision(5, 2);
                entity.Property(e => e.Mid).HasPrecision(5, 2);
                entity.Property(e => e.Final).HasPrecision(5, 2);
                entity.Property(e => e.Total).HasPrecision(6, 2);
                entity.Property(e => e.GPA).HasPrecision(3, 2);
            });

            builder.Entity<GradePolicy>(entity =>
            {
                entity.Property(e => e.Marks).HasPrecision(5, 2);
                entity.Property(e => e.GradePoint).HasPrecision(3, 2);
            });

            // 🔥 VERY IMPORTANT: Disable cascade delete
            builder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany()
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Enrollment>()
                .HasOne(e => e.CourseOffering)
                .WithMany()
                .HasForeignKey(e => e.CourseOfferingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CourseOffering>()
                .HasOne(c => c.Teacher)
                .WithMany()
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Semester>()
                .HasOne(s => s.Batch)
                .WithMany()
                .HasForeignKey(s => s.BatchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Course>()
                .HasOne(c => c.AcademicProgram)
                .WithMany()
                .HasForeignKey(c => c.AcademicProgramId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<CourseOffering>()
                .HasOne(co => co.Course)
                .WithMany()
                .HasForeignKey(co => co.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CourseOffering>()
                .HasOne(co => co.Semester)
                .WithMany()
                .HasForeignKey(co => co.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CourseOffering>()
                .HasOne(co => co.Teacher)
                .WithMany()
                .HasForeignKey(co => co.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Mark>()
                .HasOne(m => m.Enrollment)
                .WithMany()
                .HasForeignKey(m => m.EnrollmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }
}

