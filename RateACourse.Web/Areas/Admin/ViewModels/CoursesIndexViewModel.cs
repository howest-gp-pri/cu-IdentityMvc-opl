using RateACourse.Web.ViewModels;

namespace RateACourse.Web.Areas.Admin.ViewModels
{
    public class CoursesIndexViewModel
    {
        public IEnumerable<BaseViewModel> Courses { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
