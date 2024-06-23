using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using Demo.DAL.Migrations;
using Demo.PL.Helpers;
using Demo.PL.Models.UserLogins;
using Demo.PL.Models.UserRegister;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Demo.PL.Controllers.Users
{
    public class InstructorController : Controller
    {
        private readonly UserManager<DAL.Entities.ApplicationUser> _userManagerClient;
        private readonly SignInManager<DAL.Entities.ApplicationUser> _signInManagerClient;
        private readonly ICourseRepo _courseRepo;
        private readonly MvcProjectDbContext _dbContext;
        private readonly IWebHostEnvironment _environment;

        public InstructorController(UserManager<DAL.Entities.ApplicationUser> userManager
            , SignInManager<DAL.Entities.ApplicationUser> signInManager
            , ICourseRepo courseRepo
            ,MvcProjectDbContext dbContext
            , IWebHostEnvironment environment
            )
        {
            this._userManagerClient = userManager;
            this._signInManagerClient = signInManager;
            this._courseRepo = courseRepo;
            this._dbContext = dbContext;
            this._environment = environment;
        }
        #region Register
        public IActionResult InstructorRegister()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InstructorRegister(InstructorViewModel model)
        {

            if (ModelState.IsValid)//
            {
                var user = new DAL.Entities.ApplicationUser()
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    FirstName = model.FName,//Fname for Application User And First name for Modelllll 
                    LastName = model.LName,
                    Role = "Instructor",
                    PhoneNumber = model.Phone,
                    SSN = model.SSN,
                    YearsOfExperience = model.YearsOfExperience
                };

                var Result = await _userManagerClient.CreateAsync(user, model.Password);
                if (Result.Succeeded)
                {
                    return RedirectToAction(nameof(InstructorLogin));
                }
                else
                {
                    foreach (var Error in Result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, Error.Description);
                    }
                }
            }
            return View(model);
        }



        #endregion

        #region Login

        [HttpGet]
        public IActionResult InstructorLogin()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> InstructorLogin(ClientLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManagerClient.FindByEmailAsync(model.Email);
                if (User is not null && User.Role == "Instructor")
                {
                    var Result = await _userManagerClient.CheckPasswordAsync(User, model.Password);
                    if (Result)
                    {
                        var Login = await _signInManagerClient.PasswordSignInAsync(User, model.Password, model.RememberMe, false);

                        if (Login.Succeeded)
                        {
                            return RedirectToAction("InstructorHome", "Instructor");
                        }
                    }
                    else ModelState.AddModelError(string.Empty, "Pasword is not correct");
                }
                else ModelState.AddModelError(string.Empty, "Email Doesn't Exist");
            }
            return View(model);
        }
        #endregion

        #region Home

        [HttpGet]
        public IActionResult InstructorHome()
        {
            return View();
        }

        #endregion

        #region AddCourse
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DAL.Entities.Course model, IFormFile VideoFile)
        {
            model.Status = "Pending";
            if (ModelState.IsValid)
            {
                model.ImageName = DocumentSettings.UploadFille(model.Image, "Images");

                if (VideoFile != null)
                {
                    var videosPath = Path.Combine(_environment.WebRootPath, "videos");
                    if (!Directory.Exists(videosPath))
                    {
                        Directory.CreateDirectory(videosPath);
                    }

                    var filePath = Path.Combine(videosPath, VideoFile.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await VideoFile.CopyToAsync(stream);
                    }
                    model.VideoContentUrl = "/videos/" + VideoFile.FileName;
                }

                var user =await _userManagerClient.GetUserAsync(User);
                model.InstructorId = user.Id;
                model.Instructor = user;
                

                _dbContext.Add(model);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(CourseList));
              
            }
            return View(model);
        }

        #endregion

        #region Edit Course 

        // GET: Instructor/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
        // POST: Instructor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DAL.Entities.Course course, IFormFile VideoFile)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            var courseToUpdate = await _dbContext.Courses.FindAsync(id);
            if (courseToUpdate == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    courseToUpdate.Image = course.Image;
                    courseToUpdate.ImageName= DocumentSettings.UploadFille(course.Image, "Images");
                    // Update course properties
                   
                    courseToUpdate.Topic = course.Topic;
                    courseToUpdate.Description = course.Description;
                    courseToUpdate.Price = course.Price;
                    courseToUpdate.Category = course.Category;
                  

                    if (VideoFile != null)
                    {
                        var videosPath = Path.Combine(_environment.WebRootPath, "videos");
                        if (!Directory.Exists(videosPath))
                        {
                            Directory.CreateDirectory(videosPath);
                        }

                        var filePath = Path.Combine(videosPath, VideoFile.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await VideoFile.CopyToAsync(stream);
                        }
                        courseToUpdate.VideoContentUrl = "/videos/" + VideoFile.FileName;
                    }

                    _dbContext.Update(courseToUpdate);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(CourseList));
            }
            return View(course);
        }


        private bool CourseExists(int id)
        {
            return _dbContext.Courses.Any(e => e.Id == id);
        }


        #endregion

        #region Delete Course 
        // GET: Instructor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _dbContext.Courses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _dbContext.Courses.FindAsync(id);
            _dbContext.Courses.Remove(course);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(CourseList));
        }

        #endregion

        #region CourseList
        public async Task<IActionResult> CourseList() //Instructor's Course List 
        {
            var user = await _userManagerClient.GetUserAsync(User);
            var courses = _dbContext.Courses.Where(c => c.InstructorId == user.Id);
            return View(await courses.ToListAsync());
        }
        #endregion


    }
}
