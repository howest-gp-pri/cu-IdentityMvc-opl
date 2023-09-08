using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MimeKit;
using MimeKit.Text;
using RateACourse.Core.Entities;
using RateACourse.Core.Services.Interfaces;
using RateACourse.Core.Services.Models;
using RateACourse.Web.Areas.Account.ViewModels;


namespace RateACourse.Web.Areas.Account.Controllers
{
    [Area("Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IAccountService _accountService;

        public AccountController(UserManager<ApplicationUser> userManager, IEmailService emailService, SignInManager<ApplicationUser> signInManager, IAccountService accountService)
        {
            _userManager = userManager;
            _emailService = emailService;
            _signInManager = signInManager;
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.PageTitle = "Register";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AccountRegisterViewModel accountRegisterViewModel)
        {
            //errors from form validation
            if(!ModelState.IsValid)
            {
                return View(accountRegisterViewModel);
            }
            var result = await _accountService.RegisterAsync(
                new RequestRegisterModel 
                {
                    Username = accountRegisterViewModel.Email,
                    Password = accountRegisterViewModel.Password,
                    Firstname = accountRegisterViewModel.Firstname,
                    Lastname = accountRegisterViewModel.Lastname,
                });
            if (!result.IsSuccess)
            {
                //errors from service class
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
                return View(accountRegisterViewModel);
            }
            return RedirectToAction(nameof(Registered));
        }
        [HttpGet]
        public IActionResult Registered()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _accountService.Logout();
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.PageTitle = "Log in:";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountLoginViewModel accountLoginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(accountLoginViewModel);
            }
            var result = await _signInManager.PasswordSignInAsync(accountLoginViewModel.Email, accountLoginViewModel.Password, false, true);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Wrong credentials!");
                return View(accountLoginViewModel);
            }
            return RedirectToAction("Index", new { Area = "Admin", Controller = "Courses" });
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(AccountLoginViewModel accountLoginViewModel)
        //{
        //    //errors from form validation
        //    if (!ModelState.IsValid)
        //    {
        //        return View(accountLoginViewModel);
        //    }
        //    var result = await _accountService.LoginAsync(
        //        new RequestLoginModel 
        //        {
        //            Username = accountLoginViewModel.Email,
        //            Password = accountLoginViewModel.Password,
        //        });
        //    if(!result.IsSuccess)
        //    {
        //        //errors from service class
        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError("", error);
        //        }
        //        return View(accountLoginViewModel);
        //    }
        //    return RedirectToAction("Index",new {Area = "Admin", Controller = "Courses" });
        //}
        [HttpGet]
        public async Task<IActionResult> ValidateEmail(string userId,string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return NotFound();
            }
            if(await _userManager.VerifyUserTokenAsync(user,TokenOptions.DefaultProvider,"EmailConfirmation", token))
            {
                await _userManager.ConfirmEmailAsync(user,token);
            }
            return View();
        }
    }
}
