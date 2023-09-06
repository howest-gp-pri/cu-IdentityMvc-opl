using System.ComponentModel.DataAnnotations;

namespace RateACourse.Web.Areas.Account.ViewModels
{
    public class AccountRegisterViewModel : AccountLoginViewModel
    {
        
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
      
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat password")]
        public string RepeatPassword { get; set; }
    }
}
