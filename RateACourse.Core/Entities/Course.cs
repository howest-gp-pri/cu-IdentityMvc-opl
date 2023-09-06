using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateACourse.Core.Entities
{
    public class Course : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<ApplicationUser> Students { get; set; }
        public ICollection<StudentCourseReview> Reviews { get; set; }
    }
}
