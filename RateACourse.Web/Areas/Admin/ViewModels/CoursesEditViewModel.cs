using Microsoft.AspNetCore.Mvc;

namespace RateACourse.Web.Areas.Admin.ViewModels
{
    public class CoursesEditViewModel : CoursesCreateViewModel
    {
        [HiddenInput]
        public long Id { get; set; }
    }
}
