using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using Demo.PL.Models;
using Demo.PL.Models.UserLogins;
using Demo.PL.Models.UserRegister;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers.Users
{
    public class PhotographerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManagerClient;
        private readonly SignInManager<ApplicationUser> _signInManagerClient;
        private readonly MvcProjectDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PhotographerController(UserManager<ApplicationUser> userManager
            , SignInManager<ApplicationUser> signInManager
            ,MvcProjectDbContext dbContext
            , IWebHostEnvironment webHostEnvironment
            )
        {
            this._userManagerClient = userManager;
            this._signInManagerClient = signInManager;
            this._dbContext = dbContext;
            this._webHostEnvironment = webHostEnvironment;
        }

        #region Register
        public IActionResult PhotographerRegister()
        {
            var model = new PhotographerViewModel
            {
                Specialties = GetSpecialtySelectList(),
                Locations= GetLocationSelectList(),
                Avaliabilities = GetAvaliabilitySelectList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> PhotographerRegister(PhotographerViewModel model)
        {

            if (ModelState.IsValid)//
            {
                // Handle image upload
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    // Construct the file path
                    var fileName = Path.GetFileName(model.ImageFile.FileName);
                    var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "photographers");
                    var filePath = Path.Combine(directoryPath, fileName);

                    // Create the directory if it doesn't exist
                    Directory.CreateDirectory(directoryPath);

                    // Save the uploaded image to the server
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    var user = new ApplicationUser()
                    {
                        UserName = model.Email.Split('@')[0],
                        Email = model.Email,
                        FirstName = model.FName,//Fname for Application User And First name for Modelllll 
                        LastName = model.LName,
                        Role = "Photographer",
                        PhoneNumber = model.Phone,
                        SSN = model.SSN,
                        BankAccountNumber = model.BankAccountNumber,
                        PortofiloUrl = model.PortofiloUrl,
                        Specialty = model.Specialty,
                        Availiability = model.Availiability,
                        Location = model.Location,
                        ImagePath = "/images/photographers/" + fileName // Example: Store the relative path
                    };

                    var Result = await _userManagerClient.CreateAsync(user, model.Password);

                    if (Result.Succeeded)
                    {
                        return RedirectToAction(nameof(PhotographerLogin));
                    }
                    else
                    {
                        foreach (var Error in Result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, Error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        private IEnumerable<SelectListItem> GetSpecialtySelectList()
        {
            return Enum.GetValues(typeof(Specialty)).Cast<Specialty>().Select(s => new SelectListItem
            {
                Value = ((int)s).ToString(),
                Text = s.ToString()
            });
        }
        private IEnumerable<SelectListItem> GetLocationSelectList()
        {
            return Enum.GetValues(typeof(Location)).Cast<Location>().Select(s => new SelectListItem
            {
                Value = ((int)s).ToString(),
                Text = s.ToString()
            });
        }
        private IEnumerable<SelectListItem> GetAvaliabilitySelectList()
        {
            return Enum.GetValues(typeof(Availiability)).Cast<Availiability>().Select(s => new SelectListItem
            {
                Value = ((int)s).ToString(),
                Text = s.ToString()
            });
        }

        #endregion

        #region Login 

        [HttpGet]
        public IActionResult PhotographerLogin()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> PhotographerLogin(ClientLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManagerClient.FindByEmailAsync(model.Email);
                if (User is not null && User.Role == "Photographer")
                {
                    var Result = await _userManagerClient.CheckPasswordAsync(User, model.Password);
                    if (Result)
                    {
                        var Login = await _signInManagerClient.PasswordSignInAsync(User, model.Password, model.RememberMe, false);

                        if (Login.Succeeded)
                        {
                            return RedirectToAction("PhotographerHome", "Photographer");
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
        public async Task<IActionResult> PhotographerHome()
        {
           var user =await _userManagerClient.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("photographerlogin", "photographer");
            }
            return View(user);
        }

        #endregion

        #region Profile

        [HttpGet]
        public async Task<IActionResult> MyProfile(string id)
        {
            var user = await _dbContext.Users
         .OfType<ApplicationUser>()
         .Include(u => u.PhotographerImages)
         .FirstOrDefaultAsync(u => u.Id == id);

            return View(user);
        }

        #endregion

        #region Upload Images

        [HttpGet]
        public IActionResult UploadImage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage( List<IFormFile> imageFiles)
        {
            if (imageFiles == null || !imageFiles.Any())
            {
                ModelState.AddModelError("ImageFiles", "Please select at least one image to upload.");
                return View(); // Return the view with validation error messages
            }

            foreach (var imageFile in imageFiles)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    // Save image data to the database
                    var photographerImage = new PhotographerImages
                    {
                        ImagePath = uniqueFileName,
                        PhotographerId = User.FindFirstValue(ClaimTypes.NameIdentifier) // Assuming you're using ASP.NET Core Identity
                    };
                    _dbContext.PhotographerImages.Add(photographerImage);
                }
            }

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("MyProfile", "Photographer", new { id = User.FindFirstValue(ClaimTypes.NameIdentifier) });
        }


        #endregion

        #region Edit profile 
        [HttpGet]
        public IActionResult EditProfile()
        {
            // Retrieve the current user (photographer)
            var user = _userManagerClient.GetUserAsync(User).Result;
            if (user == null)
            {
                return NotFound();
            }

            // Populate the view model with the current user's profile information
            var model = new EditProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.PhoneNumber,
                LinkedInUrl = user.PortofiloUrl,//Assuming this property exists in your ApplicationUser model
                ImagePath = user.ImagePath
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManagerClient.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName =model.FirstName;
            
            user.PhoneNumber = model.Phone;
            user.LastName = model.LastName;
            user.PortofiloUrl = model.LinkedInUrl;

            if (model.ProfileImage != null)
            {
                var imagePath = await SaveProfileImageAsync(model.ProfileImage);
                user.ImagePath = imagePath;
            }

            var result = await _userManagerClient.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("MyProfile", new { id = user.Id });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
        }

        private async Task<string> SaveProfileImageAsync(IFormFile profileImage)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + profileImage.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await profileImage.CopyToAsync(fileStream);
            }

            return "/images/" + uniqueFileName;
        }


        #endregion
    }


}
