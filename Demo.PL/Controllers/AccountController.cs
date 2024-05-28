using Demo.DAL.Entities;
using Demo.PL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Client> _userManagerClient;
        private readonly SignInManager<Client> _signInManagerClient;

        public AccountController(UserManager<Client> userManager
            ,SignInManager<Client> signInManager
            ) 
        {
            this._userManagerClient = userManager;
            this._signInManagerClient = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            //  List<string> Roles = new List<string>()
            //  {
            //        "Photographer",
            //        "Client",
            //        "Instructor",
            //        "Seller"

            //  };
            //ViewBag.Roles = new SelectList(Roles);

            return View();
        }

        [HttpPost]
        //public async Task<IActionResult> Register(RegisterViewModel model)
        //{ 

        //    if (ModelState.IsValid)//
        //    {
        //        var user = new ApplicationUser()
        //        {
        //            UserName = model.Email.Split('@')[0],
        //            Email = model.Email,
        //            FName = model.FName,//Fname for Application User And First name for Modelllll 
        //            LName = model.LName
        //        };

        //        var Result = await _userManager.CreateAsync(user, model.Password);
        //        if (Result.Succeeded)
        //        {
        //            return RedirectToAction(nameof(Login));
        //        }
        //        else
        //        {
        //            foreach (var Error in Result.Errors)
        //            {
        //                ModelState.AddModelError(string.Empty, Error.Description);
        //            }
        //        }
        //    } 
        //    return View(model);
        //}

        public async Task<IActionResult> Register(RegisterViewModel model)
        {

            if (ModelState.IsValid)//
            {
                var user = new Client()
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    FirstName = model.FName,//Fname for Application User And First name for Modelllll 
                    LastName = model.LName,
                    Role = "Client",
                    PhoneNumber=model.Phone,
                    
                };

                var Result = await _userManagerClient.CreateAsync(user, model.Password);
                if (Result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
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

        [HttpGet]
        public ActionResult LoginAsync()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
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
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else ModelState.AddModelError(string.Empty, "Pasword is not correct");
                }
                else ModelState.AddModelError(string.Empty, "Email Doesn't Exist");
            }
            return View(model);
        }
        /////////// Sign OUT 
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        //////// Forget Password////
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(model.Email);
                if (User is not null)
                {
                    // valid fore one user for one only time 
                    var token = await _userManager.GeneratePasswordResetTokenAsync(User);
                    var ResetPasswordLink = Url.Action("ResetPassword", "Account", new { email = User.Email, Token = token }, Request.Scheme);
                    /// Send Email
                    var email = new Email()
                    {
                        Subject = "Reset Password",
                        To = model.Email,
                        Body = ResetPasswordLink
                    };
                    EmailSettings.SendEmail(email);
                    return RedirectToAction(nameof(CheckYourInbox));

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email is not Exists");
                }

            }
            return View("ForgetPassword", model);

        }
        public IActionResult CheckYourInbox()
        {
            return View();
        }
        //////Reset password
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                string email = TempData["email"] as string;
                string token = TempData["token"] as string;
                var User = await _userManager.FindByEmailAsync(email);
                var Result = await _userManager.ResetPasswordAsync(User, token, model.NewPassword);
                if (Result.Succeeded)
                    return RedirectToAction(nameof(Login));
                else
                    foreach (var error in Result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

    }
}
