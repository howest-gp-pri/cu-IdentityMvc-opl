using System.ComponentModel.DataAnnotations;

namespace RateACourse.Web.Areas.Account.ViewModels
{
    public class AccountRegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat password")]
        public string RepeatPassword { get; set; }
    }
}
