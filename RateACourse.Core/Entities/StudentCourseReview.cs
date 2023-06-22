using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateACourse.Core.Entities
{
    public class StudentCourseReview
    {
        public Course Course { get; set; }
        public Student Student { get; set; }
        public long StudentId { get; set; }
        public long CourseId { get; set; }
        public int Score { get; set; }
        public string ReviewText { get; set; }
    }
}
