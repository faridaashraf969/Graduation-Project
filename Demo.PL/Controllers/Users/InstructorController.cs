using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using Demo.DAL.Migrations;
using Demo.PL.Helpers;
using Demo.PL.Models.UserLogins;
using Demo.PL.Models.UserRegister;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
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

        public InstructorController(UserManager<DAL.Entities.ApplicationUser> userManager
            , SignInManager<DAL.Entities.ApplicationUser> signInManager
            , ICourseRepo courseRepo
            ,MvcProjectDbContext dbContext
            )
        {
            this._userManagerClient = userManager;
            this._signInManagerClient = signInManager;
            this._courseRepo = courseRepo;
            this._dbContext = dbContext;
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
                if (User is not null)
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
        public async Task<IActionResult> Create(DAL.Entities.Course model)
        {
            model.Status = "Pending";
            if (ModelState.IsValid)
            {
                model.ImageName = DocumentSettings.UploadFille(model.Image, "Images");

                var user =await _userManagerClient.GetUserAsync(User);
                model.InstructorId = user.Id;
                model.Instructor = user;

                _courseRepo.Add(model);
                return RedirectToAction(nameof(CourseList));
              
            }
            return View(model);
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
