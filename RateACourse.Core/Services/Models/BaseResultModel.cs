using RateACourse.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateACourse.Core.Services.Models
{
    public class BaseResultModel
    {
        
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
