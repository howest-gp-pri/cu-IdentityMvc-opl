using RateACourse.Core.Entities;
using RateACourse.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateACourse.Core.Services.Interfaces
{
    public interface ICourseService
    {
        Task<CourseServiceResultModel<Course>> GetAllAsync();
        Task<CourseServiceResultModel<Course>> GetByIdAsync(long id);
        Task<CourseServiceResultModel<Course>> DeleteAsync(long id);
        Task<CourseServiceResultModel<Course>> UpdateAsync(long id, string name);
        Task<CourseServiceResultModel<Course>> CreateAsync(string name);
        Task<CourseServiceResultModel<Course>> RateAsync(RequestRateCourseModel requestRateCourseModel);
        Task<bool> CheckIfExistsAsync(long id);
    }
}
