using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseRepo _courseRepo;
        private readonly MvcProjectDbContext _dbContext;

        //constractor
        public CourseController(ICourseRepo courseRepo
            ,MvcProjectDbContext dbContext)
        {
           _courseRepo = courseRepo;
            this._dbContext = dbContext;
        }
        public  IActionResult Index() //store of courses
        {
            var courses = _dbContext.Courses.Include(c => c.Instructor).Where(c => c.Status == "Approved").ToList();
            return View(courses);
        }

        public IActionResult AddToCart()
        {
            return View();  
        }

        public IActionResult Delete()
        {
            return View();
        }
    }
}
