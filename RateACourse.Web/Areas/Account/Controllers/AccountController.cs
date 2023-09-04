using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RateACourse.Core.Entities;
using RateACourse.Web.Areas.Account.ViewModels;

namespace RateACourse.Web.Areas.Account.Controllers
{
    [Area("Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        
        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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
            if(!ModelState.IsValid)
            {
                return View(accountRegisterViewModel);
            }
            if (await _userManager.FindByNameAsync(accountRegisterViewModel.Username) == null)
            {
                var user = new ApplicationUser 
                { 
                    UserName = accountRegisterViewModel.Username,
                    Firstname = accountRegisterViewModel.Firstname,
                    Lastname = accountRegisterViewModel.Lastname,
                    Email = accountRegisterViewModel.Username,
                    EmailConfirmed = true
                };
                await _userManager.CreateAsync(user,accountRegisterViewModel.Password);
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var result = await _userManager.VerifyUserTokenAsync(user,TokenOptions.DefaultProvider, "EmailConfirmation", token);
                return RedirectToAction(nameof(Registered));
            }
            return RedirectToAction(nameof(Index),"Courses");
            
        }
        [HttpGet]
        public IActionResult Registered()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ValidateAccount(string userId,string token)
        {
            var user = await _userManager.FindByNameAsync(userId);
            if(user == null)
            {
                NotFound();
            }
            if(await _userManager.VerifyUserTokenAsync(user,TokenOptions.DefaultProvider,"EmailConfirmation", token))
            {
                await _userManager.ConfirmEmailAsync(user,token);
            }
            return View();
        }
    }
}
