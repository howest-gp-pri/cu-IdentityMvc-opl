using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RateACourse.Core.Entities;
using RateACourse.Web.Areas.Account.ViewModels;
using System.Linq;

namespace RateACourse.Web.Areas.Account.Controllers
{
    [Area("Account")]
    //[Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            UsersIndexViewModel usersIndexViewModel = new UsersIndexViewModel();
            var users = _userManager.Users.ToList();
            usersIndexViewModel.Users = users.Select
                (
                    u => new UserViewModel 
                    {
                        Id = u.Id,
                        Name = $"{u.Firstname} {u.Lastname}",
                        Roles = _userManager.GetRolesAsync(u).Result
                    }
                );
            return View(usersIndexViewModel);
        }
        public async Task<IActionResult> RemoveRole(string userId,string role)
        {
            var user  = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return NotFound();
            }
            if(!await _roleManager.RoleExistsAsync(role))
            {
                return NotFound();
            }
            var result = await _userManager.RemoveFromRoleAsync(user, role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            return View("Error");
        }
        [HttpGet]
        public async Task<IActionResult> AddRole(string id)
        {
            var roles = await _roleManager.Roles.ToListAsync();
            UsersAddRoleViewModel usersAddRoleViewModel = new();
            usersAddRoleViewModel.Roles = roles.Select(r => new SelectListItem 
            {
                Text = r.Name,
                Value = r.Name
            });
            usersAddRoleViewModel.Id = id;
            return View(usersAddRoleViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(UsersAddRoleViewModel usersAddRoleViewModel)
        {
            var user = await _userManager.FindByIdAsync(usersAddRoleViewModel.Id);
            if(user == null
                ||
                !await _roleManager.RoleExistsAsync(usersAddRoleViewModel.RoleId))
            {
                return NotFound();
            }
            await _userManager.AddToRoleAsync(user,usersAddRoleViewModel.RoleId);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> CreateRole()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            UsersCreateRoleViewModel usersCreateRoleViewModel = new();
            usersCreateRoleViewModel.Roles = roles.Select(r => r.Name);
            return View(usersCreateRoleViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(UsersCreateRoleViewModel usersCreateRoleViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(usersCreateRoleViewModel);
            }
            if(await _roleManager.RoleExistsAsync(usersCreateRoleViewModel.Role))
            {
                ModelState.AddModelError("Role", "Role exists");
                usersCreateRoleViewModel.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
                return View(usersCreateRoleViewModel);
            }
            await _roleManager.CreateAsync(new IdentityRole 
            {
                Name = usersCreateRoleViewModel.Role,
            });
            return RedirectToAction("CreateRole");
        }
        [HttpGet]
        public IActionResult ConfirmRemoveUserFromRole(string userId,string roleId)
        {
            UsersRemoveUserFromRoleViewModel usersRemoveUserFromRoleViewModel = new();

            return View(usersRemoveUserFromRoleViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveUserFromRole(UsersRemoveUserFromRoleViewModel usersRemoveUserFromRoleViewModel)
        {
            var user = await _userManager.FindByIdAsync(usersRemoveUserFromRoleViewModel.UserId);
            var role = await _roleManager.FindByIdAsync(usersRemoveUserFromRoleViewModel.RoleId);
            if(user == null || role == null)
            {
                return NotFound();
            }
            await _userManager.RemoveFromRoleAsync(user, role.Name);
            return View("Index");
        }
    }
}
