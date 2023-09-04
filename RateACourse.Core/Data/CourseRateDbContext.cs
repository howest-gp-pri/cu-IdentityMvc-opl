using RateACourse.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RateACourse.Core.Data
{
    public class CourseRateDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentCourseReview> StudentCourseReviews { get; set; }

        public CourseRateDbContext(DbContextOptions<CourseRateDbContext> options) : base(options)
        {
        
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //course table
            modelBuilder.Entity<Course>()
                .Property(c => c.CourseName)
                .IsRequired()
                .HasMaxLength(150);
            //student table
            modelBuilder.Entity<Student>()
                .Property(s => s.FirstName)
                .IsRequired()
                .HasMaxLength(150);
            modelBuilder.Entity<Student>()
                .Property(s => s.LastName)
                .IsRequired()
                .HasMaxLength(150);
            //configure StudentCourseReview
            modelBuilder.Entity<StudentCourseReview>()
                .HasKey(sc => new { sc.CourseId, sc.StudentId });
            modelBuilder.Entity<StudentCourseReview>()
                .HasOne(cs => cs.Student)
                .WithMany(s => s.Reviews)
                .HasForeignKey(s => s.StudentId);
            modelBuilder.Entity<StudentCourseReview>()
                .HasOne(cs => cs.Course)
                .WithMany(c => c.Reviews)
                .HasForeignKey(cs => cs.CourseId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
