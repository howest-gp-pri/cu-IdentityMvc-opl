using RateACourse.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace RateACourse.Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourseReview> StudentCourseReviews { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //course table
            modelBuilder.Entity<Course>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(150);
            //student table
            modelBuilder.Entity<ApplicationUser>()
                .Property(s => s.Firstname)
                .IsRequired()
                .HasMaxLength(150);
            modelBuilder.Entity<ApplicationUser>()
                .Property(s => s.Lastname)
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
            const string AdminRoleId = "00000000-0000-0000-0000-000000000001";
            const string AdminRoleName = "Admin";
            const string AdminUserId = "00000000-0000-0000-0000-000000000001";
            const string AdminUserName = "admin@rateamovie.be";
            const string AdminFirstname = "Mike";
            const string AdminLastname = "Admin";
            const string AdminUserPassword = "Test123?"; // For demo purposes only! Don't do this in real application!

            IPasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>(); // Identity password hasher

            ApplicationUser adminApplicationUser = new ApplicationUser
            {
                Id = AdminUserId,
                UserName = AdminUserName,
                NormalizedUserName = AdminUserName.ToUpper(),
                Email = AdminUserName,
                NormalizedEmail = AdminUserName.ToUpper(),
                EmailConfirmed = true,
                Firstname = AdminFirstname,
                Lastname = AdminLastname,
                SecurityStamp = "VVPCRDAS3MJWQD5CSW2GWPRADBXEZINA", //Random string
                ConcurrencyStamp = "c8554266-b401-4519-9aeb-a9283053fc58", //Random guid string
            };

            adminApplicationUser.PasswordHash = passwordHasher.HashPassword(adminApplicationUser, AdminUserPassword);

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = AdminRoleId,
                Name = AdminRoleName,
                NormalizedName = AdminRoleName.ToUpper()
            });

            modelBuilder.Entity<ApplicationUser>().HasData(adminApplicationUser);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = AdminRoleId,
                UserId = AdminUserId
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
