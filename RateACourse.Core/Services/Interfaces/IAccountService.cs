using RateACourse.Core.Entities;
using RateACourse.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateACourse.Core.Services.Interfaces
{
    public interface IAccountService
    {
        Task<BaseResultModel> RegisterAsync(RequestRegisterModel requestRegisterModel);
        Task<BaseResultModel> LoginAsync(RequestLoginModel requestLoginModel);
        Task Logout();
    }
}
