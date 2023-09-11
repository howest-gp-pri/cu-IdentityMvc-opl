using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using RateACourse.Core.Entities;
using RateACourse.Web.Areas.Account.ViewModels;


namespace RateACourse.Web.Areas.Account.Controllers
{
    [Area("Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LinkGenerator _linkGenerator;

        public AccountController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _linkGenerator = linkGenerator;
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
            if (!ModelState.IsValid)
            {
                return View(accountRegisterViewModel);
            }
            if (await _userManager.FindByNameAsync(accountRegisterViewModel.Email) == null)
            {
                var user = new ApplicationUser
                {
                    UserName = accountRegisterViewModel.Email,
                    Firstname = accountRegisterViewModel.Firstname,
                    Lastname = accountRegisterViewModel.Lastname,
                    Email = accountRegisterViewModel.Email,
                    EmailConfirmed = false
                };
                await _userManager.CreateAsync(user, accountRegisterViewModel.Password);
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = _linkGenerator.GetUriByAction
                    (
                        action: "ValidateEmail",
                        controller: "Account",
                        scheme: _httpContextAccessor.HttpContext.Request.Scheme,
                        host: _httpContextAccessor.HttpContext.Request.Host,
                        values: new { Area = "Account", userId = user.Id, token = token }
                    );
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("admin@rateACourse.com"));
                email.To.Add(MailboxAddress.Parse(user.Email));
                email.Subject = "Confirm your emailaddress";
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = $"<h5>Please confirm your emailadress</h5>" +
                           $"<p>Please confirm your emailadress by clicking " +
                           $"<a href='{confirmationLink}'>here</a>",
                };
                using var smtpClient = new SmtpClient();
                await smtpClient.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync("gradProgPriTest@gmail.com", "owjxlealkiucmsbu");
                var result = await smtpClient.SendAsync(email);
                await smtpClient.DisconnectAsync(true);
                return RedirectToAction(nameof(Registered));
            }
            ModelState.AddModelError("Email", "This email is already taken!");
            return View(accountRegisterViewModel);
        }
        [HttpGet]
        public IActionResult Registered()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
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
            return RedirectToAction("Index","Courses", new { Area = "" });
        }
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
        [HttpGet]
        public IActionResult AccesDenied()
        {
            return View();
        }
    }
}
