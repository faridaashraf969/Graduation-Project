using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.BillingPortal;
using System.Collections.Generic;
using System.Linq;

























using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseRepo _courseRepo;
        private readonly MvcProjectDbContext _dbContext;
       

        public Course Course { get; private set; }
        public int Quantity { get; private set; }

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

        public IActionResult AddToCart(int CourseId)
        {
            //if (Session["cart"] == null)
            //{
            //    var cart = new List<CartItem>();
            //    var course = _dbContext.Courses.Find(CourseId);
            //    cart.Add(new CartItem()
            //    {
            //        Course = course,
            //        Quantity = 1
            //    });
            //    //Session["cart"] = cart;
            //}else
            //{
            //    List<CartItem> cart = ( List<CartItem>) Session["cart"]
            //    var course = _dbContext.Courses.Find(CourseId);
            //    cart.Add(new CartItem()
            //    {
            //        Course = course,
            //        Quantity = 1
            //    });
            //    //Session["cart"] = cart;
            //}

            return Redirect("Index");  
        }

        public IActionResult Delete()
        {
            return View();
        }
    }
}
