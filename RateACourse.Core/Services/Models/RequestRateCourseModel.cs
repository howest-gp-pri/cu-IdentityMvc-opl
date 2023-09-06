using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateACourse.Core.Services.Models
{
    public class RequestRateCourseModel
    {
        public long CourseId{ get; set; }
        public int Score { get; set; }
        public string StudentId { get; set; }
        public string Review { get; set; }
    }
}
