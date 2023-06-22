using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateACourse.Core.Entities
{
    public class Course : BaseEntity
    {
        public string CourseName { get; set; }
        public ICollection<Student> Students { get; set; }
        public ICollection<StudentCourseReview> Reviews { get; set; }
    }
}
