using Microsoft.AspNetCore.Mvc;

namespace RateACourse.Web.Controllers
{
    public class CoursesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Rate(int id) 
        {
            return View();
        }
    }
}
