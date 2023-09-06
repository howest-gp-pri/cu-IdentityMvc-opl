using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateACourse.Core.Services.Models
{
    public class RequestRegisterModel : RequestLoginModel
    {
        
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
