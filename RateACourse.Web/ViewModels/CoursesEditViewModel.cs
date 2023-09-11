using Microsoft.AspNetCore.Mvc;

namespace RateACourse.Web.ViewModels
{
    public class CoursesEditViewModel : CoursesCreateViewModel
    {
        [HiddenInput]
        public long Id { get; set; }
    }
}
