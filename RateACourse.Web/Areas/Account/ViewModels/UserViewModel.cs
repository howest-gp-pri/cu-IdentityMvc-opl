using RateACourse.Web.ViewModels;

namespace RateACourse.Web.Areas.Account.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        public new string Id { get; set; }
        public IList<string> Roles { get; set; }
    }
}
