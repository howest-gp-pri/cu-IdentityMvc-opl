using System.ComponentModel.DataAnnotations;

namespace RateACourse.Web.Areas.Admin.ViewModels
{
    public class CoursesCreateViewModel
    {
        [Required]
        [Display(Name = "Course")]
        public string Name { get; set; }
    }
}
