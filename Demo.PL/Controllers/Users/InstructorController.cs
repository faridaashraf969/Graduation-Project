using Demo.DAL.Entities;
using Demo.PL.Models.UserLogins;
using Demo.PL.Models.UserRegister;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers.Users
{
    public class InstructorController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManagerClient;
        private readonly SignInManager<ApplicationUser> _signInManagerClient;

        public InstructorController(UserManager<ApplicationUser> userManager
            , SignInManager<ApplicationUser> signInManager
            )
        {
            this._userManagerClient = userManager;
            this._signInManagerClient = signInManager;
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
                var user = new ApplicationUser()
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    FirstName = model.FName,//Fname for Application User And First name for Modelllll 
                    LastName = model.LName,
                    Role = "Instructor",
                    PhoneNumber = model.Phone,
                    SSN = model.SSN,
                    YearsOfExperience=model.YearsOfExperience
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

    }
}
