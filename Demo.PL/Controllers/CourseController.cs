using Demo.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseRepo _courseRepo;
        //constractor
        public CourseController(ICourseRepo courseRepo)
        {
           _courseRepo = courseRepo;
        }
        public  IActionResult Index()
        {
            var courses = _courseRepo.GetAll();
            return View(courses);
        }
    }
}
