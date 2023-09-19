using Microsoft.AspNetCore.Mvc;

namespace RateACourse.Web.Areas.Account.ViewModels
{
    public class UsersRemoveUserFromRoleViewModel
    {
        [HiddenInput]
        public string UserId { get; set; }
        [HiddenInput]
        public string RoleId { get; set; }
    }
}
