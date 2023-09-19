using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RateACourse.Web.Areas.Account.ViewModels
{
    public class UsersAddRoleViewModel
    {
        [HiddenInput]
        public string Id { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
        [Display(Name = "Roles")]
        public string RoleId { get; set; }
    }
}
