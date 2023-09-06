using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using RateACourse.Core.Entities;
using RateACourse.Core.Services.Interfaces;
using RateACourse.Core.Services.Models;

using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RateACourse.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly LinkGenerator _linkGenerator;

        public AccountService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IHttpContextAccessor httpContextAccessor,
            IEmailService emailService,
            LinkGenerator linkGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _linkGenerator = linkGenerator;
        }

        public async Task<BaseResultModel> LoginAsync(RequestLoginModel requestLoginModel)
        {
            var result = await _signInManager.PasswordSignInAsync(requestLoginModel.Username, requestLoginModel.Password, false, true);
            if (result.Succeeded == false)
            {
                return new BaseResultModel
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Wrong credentials!" }
                };
            }
            return new BaseResultModel { IsSuccess = true };
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<BaseResultModel> RegisterAsync(RequestRegisterModel requestRegisterModel)
        {
            if (await _userManager.FindByNameAsync(requestRegisterModel.Username) == null)
            {
                var user = new ApplicationUser
                {
                    UserName = requestRegisterModel.Username,
                    Firstname = requestRegisterModel.Firstname,
                    Lastname = requestRegisterModel.Lastname,
                    Email = requestRegisterModel.Username,
                    EmailConfirmed = false
                };
                await _userManager.CreateAsync(user, requestRegisterModel.Password);
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = GenerateConfirmationLink(user, token);
                var result = await _emailService
                    .SendConfirmationMailAsync(user.Id, user.Email, token, confirmationLink);
                if (result.IsSuccess)
                    return new BaseResultModel { IsSuccess = true };
                else
                    return new BaseResultModel
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "Something went wrong" }
                    };
            }
            return new BaseResultModel
            {
                IsSuccess = false,
                Errors = new List<string> { "Username taken." }
            };
        }
        private string GenerateConfirmationLink(ApplicationUser applicationUser, string token)
        {
            var confirmationlink = _linkGenerator.GetUriByAction
                    (
                        action: "ValidateEmail",
                        controller: "Account",
                        scheme: _httpContextAccessor.HttpContext.Request.Scheme,
                        host: _httpContextAccessor.HttpContext.Request.Host,
                        values: new { Area = "Account", userId = applicationUser.Id, token = token }
                    );
            return confirmationlink;
        }
    }
}
