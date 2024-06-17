using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.BillingPortal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseRepo _courseRepo;
        private readonly MvcProjectDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public Course Course { get; private set; }
        public int Quantity { get; private set; }

        //constractor
        public CourseController(ICourseRepo courseRepo
            , MvcProjectDbContext dbContext
            , UserManager<ApplicationUser> userManager
            )
        {
            _courseRepo = courseRepo;
            this._dbContext = dbContext;
            this._userManager = userManager;
        }
        public IActionResult Index() //store of courses
        {
            var courses = _dbContext.Courses.Include(c => c.Instructor).Where(c => c.Status == "Approved").ToList();
            return View(courses);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _dbContext.Courses.Include(c => c.Instructor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Client/Enroll/5
        public async Task<IActionResult> Enroll(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _dbContext.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var enrollment = new Enrollment
            {
                CourseId = course.Id,
                UserId = userId,
                EnrollmentDate = DateTime.Now
            };

            _dbContext.Enrollments.Add(enrollment);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Payment), new { id = course.Id });
        }

        // GET: Client/Payment/5
        public async Task<IActionResult> Payment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _dbContext.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            // Simulate payment process
            return View(course);

        }
        // GET: Client/EnterPaymentDetails/5
        public async Task<IActionResult> EnterPaymentDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _dbContext.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }
        // POST: Client/ProcessPayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayment(int id, string cardNumber, string cardHolderName, string expirationDate, string cvv)
        {
            var userId = _userManager.GetUserId(User);

            var enrollment = await _dbContext.Enrollments
                .FirstOrDefaultAsync(e => e.CourseId == id && e.UserId == userId);
            if (enrollment == null)
            {
                return NotFound();
            }

            // Simulate payment processing
            // This is where you would integrate with a real payment gateway

            // For simulation, mark the enrollment as paid
            enrollment.IsPaid = true;
            _dbContext.Update(enrollment);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Watch), new { id });
        }

        //// POST: Client/ConfirmPayment/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ConfirmPayment(int id)
        //{
        //    var userId = _userManager.GetUserId(User);

        //    var enrollment = await _dbContext.Enrollments
        //        .FirstOrDefaultAsync(e => e.CourseId == id && e.UserId == userId);
        //    if (enrollment == null)
        //    {
        //        return NotFound();
        //    }
        //    // Mark the enrollment as paid
        //    enrollment.IsPaid = true;
        //    _dbContext.Update(enrollment);
        //    await _dbContext.SaveChangesAsync();

        //    return RedirectToAction(nameof(Watch), new { id });
        //}

        // GET: Client/Watch/5
        public async Task<IActionResult> Watch(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var enrollment = await _dbContext.Enrollments
                .FirstOrDefaultAsync(e => e.CourseId == id && e.UserId == userId && e.IsPaid);

            if (enrollment == null)
            {
                return NotFound();
            }
            var course = await _dbContext.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }
    }
}
