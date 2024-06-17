using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using Demo.PL.Models;
using Demo.PL.Models.UserLogins;
using Demo.PL.Models.UserRegister;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers.Users
{
    public class ClientController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManagerClient;
        private readonly SignInManager<ApplicationUser> _signInManagerClient;
        private readonly MvcProjectDbContext _dbContext;

        public ClientController(UserManager<ApplicationUser> userManager
            , SignInManager<ApplicationUser> signInManager
             ,MvcProjectDbContext dbContext
            )
        {
            this._userManagerClient = userManager;
            this._signInManagerClient = signInManager;
            this._dbContext = dbContext;
        }

        #region Register
        [HttpGet]
        public IActionResult ClientRegister()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ClientRegister(ClientRegisterViewModel model)
        {

            if (ModelState.IsValid)//
            {
                var user = new ApplicationUser()
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    FirstName = model.FName,//Fname for Application User And First name for Modelllll 
                    LastName = model.LName,
                    Role = "Client",
                    PhoneNumber = model.Phone,
                    Address=model.Address

                };

                var Result = await _userManagerClient.CreateAsync(user, model.Password);
                if (Result.Succeeded)
                {
                    return RedirectToAction(nameof(ClientLogin));
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
        
        /////////
       
        #region Login

        [HttpGet]
        public IActionResult ClientLogin()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ClientLogin(ClientLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManagerClient.FindByEmailAsync(model.Email);
                if (User is not null && User.Role=="Client")
                {
                    var Result = await _userManagerClient.CheckPasswordAsync(User, model.Password);
                    if (Result)
                    {
                        var Login = await _signInManagerClient.PasswordSignInAsync(User, model.Password, model.RememberMe, false);

                        if (Login.Succeeded)
                        {
                            return RedirectToAction("ClientHome", "Client");
                        }
                    }
                    else ModelState.AddModelError(string.Empty, "Pasword is not correct");
                }
                else ModelState.AddModelError(string.Empty, "Email Doesn't Exist");
            }
            return View(model);
        }
        #endregion

        ////////
        ///
        #region Home

        [HttpGet]
        public IActionResult ClientHome()
        {
            return View();
        }

        #endregion

        #region MyCourses 

        // GET: Client/MyCourses
        public async Task<IActionResult> MyCourses()
        {
            var userId = _userManagerClient.GetUserId(User);
            var enrollments = await _dbContext.Enrollments
                .Include(e => e.Course)
                .Where(e => e.UserId == userId && e.IsPaid)
                .ToListAsync();

            return View(enrollments);
        }


        #endregion
    }
}
