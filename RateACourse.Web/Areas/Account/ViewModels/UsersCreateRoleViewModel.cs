using System.ComponentModel.DataAnnotations;

namespace RateACourse.Web.Areas.Account.ViewModels
{
    public class UsersCreateRoleViewModel
    {
        [Required]
        public string Role { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
