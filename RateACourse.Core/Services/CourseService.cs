using Microsoft.EntityFrameworkCore;
using RateACourse.Core.Data;
using RateACourse.Core.Entities;
using RateACourse.Core.Services.Interfaces;
using RateACourse.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateACourse.Core.Services
{
    public class CourseService : ICourseService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CourseService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<CourseServiceResultModel<Course>> CreateAsync(string name)
        {
            if(await _applicationDbContext.Courses.AnyAsync(c => c.Name.ToUpper().Equals(name.ToUpper())))
            {
                return new CourseServiceResultModel<Course> 
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Course exists!" }
                };
            }
            var course = new Course
            {
                Name = name,
            };
            await _applicationDbContext.Courses.AddAsync(course);
            await _applicationDbContext.SaveChangesAsync();
            return new CourseServiceResultModel<Course> { IsSuccess = true };
        }

        public async Task<CourseServiceResultModel<Course>> DeleteAsync(long id)
        {
            var course = await _applicationDbContext
                .Courses
                .FirstOrDefaultAsync(c => c.Id == id);
            if (course == null)
            {
                return new CourseServiceResultModel<Course>
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Unkown course!" }
                };
            }
            _applicationDbContext.Courses.Remove(course);
            await _applicationDbContext.SaveChangesAsync();
            return new CourseServiceResultModel<Course> { IsSuccess = true };
        }

        public async Task<CourseServiceResultModel<Course>> GetAllAsync()
        {
            var courses = await _applicationDbContext.Courses.ToListAsync();
            if(courses.Count == 0)
            {
                return new CourseServiceResultModel<Course>
                {
                    IsSuccess = false,
                    Errors = new List<string> { "No courses in databases!" }
                };
            }
            return new CourseServiceResultModel<Course>  
            {
                IsSuccess = true,
                Items = courses
            };
        }

        public async Task<CourseServiceResultModel<Course>> GetByIdAsync(long id)
        {
            var course = await _applicationDbContext.Courses
                .FirstOrDefaultAsync(c => c.Id == id);
            if (course == null)
            {
                return new CourseServiceResultModel<Course>
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Course not found!" }
                };
            }
            return new CourseServiceResultModel<Course>
            {
                IsSuccess = true,
                Items = new List<Course> { course }
            };
        }

        public async Task<CourseServiceResultModel<Course>> RateAsync(RequestRateCourseModel requestRateCourseModel)
        {
            var course = await _applicationDbContext.Courses
                .FirstOrDefaultAsync(c => c.Id == requestRateCourseModel.CourseId);
            var user = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Id == requestRateCourseModel.StudentId);
            if (course == null || user == null)
            {
                return new CourseServiceResultModel<Course>
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Something went wrong!" }
                };
            }
            var review = new StudentCourseReview 
            {
                CourseId = requestRateCourseModel.CourseId,
                StudentId = requestRateCourseModel.StudentId,
                Score = requestRateCourseModel.Score,
                ReviewText = requestRateCourseModel.Review
            };
            await _applicationDbContext.StudentCourseReviews.AddAsync(review);
            await _applicationDbContext.SaveChangesAsync();
            return new CourseServiceResultModel<Course>{ IsSuccess = true };
        }

        public async Task<CourseServiceResultModel<Course>> UpdateAsync(long id, string name)
        {
            var course = await _applicationDbContext.Courses
                .FirstOrDefaultAsync(c => c.Id == id);
            if (course == null)
            {
                return new CourseServiceResultModel<Course>
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Course not found!" }
                };
            }
            //update
            course.Name = name;
            return await SaveChangesAsync();
            
        }
        private async Task<CourseServiceResultModel<Course>> SaveChangesAsync()
        {
            try
            {
                await _applicationDbContext.SaveChangesAsync();
                return new CourseServiceResultModel<Course> { IsSuccess = true };
            }
            catch (DbUpdateException dbUpdateException)
            {
                Console.WriteLine(dbUpdateException.Message);
                return new CourseServiceResultModel<Course>
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Some unkown error occurred. Please try again later." }
                };
            }
        }
    }
}
