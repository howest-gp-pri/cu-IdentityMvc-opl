using RateACourse.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateACourse.Core.Services.Models
{
    public class CourseServiceResultModel<T>: BaseResultModel where T : BaseEntity
    {
        public IEnumerable<T> Items { get; set; }
    }
}
