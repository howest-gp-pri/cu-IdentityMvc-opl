using RateACourse.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RateACourse.Core.Services.Interfaces
{
    public interface IEmailService
    {
        Task<BaseResultModel> SendConfirmationMailAsync(string userId,string userEmail, string token,string confirmationLink);
    }
}
