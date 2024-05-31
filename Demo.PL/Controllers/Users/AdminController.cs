using Demo.BLL.Interfaces;
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
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IProductRepo _productRepo;
        private readonly ICourseRepo _courseRepo;
        private readonly MvcProjectDbContext _dbContext;
        private readonly ICategoryRepo _categoryRepo;

        public AdminController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
            ,IProductRepo productRepo
            ,ICourseRepo courseRepo
            ,MvcProjectDbContext dbContext
            ,ICategoryRepo categoryRepo) 
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._productRepo = productRepo;
            this._courseRepo = courseRepo;
            this._dbContext = dbContext;
            this._categoryRepo = categoryRepo;
        }
        #region Admin Home
        public IActionResult AdminHome() //Admin Page (Mai Salah 3la Telegram)
        {
            return View();
        }
        #endregion

        #region Admin Register
        public async Task<IActionResult> AdminRegister(AddAdmin model)
        {

            if (ModelState.IsValid)//
            {
                var user = new ApplicationUser()
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    Role = "Admin",

                    

                };

                var Result = await _userManager.CreateAsync(user, model.Password);
                if (Result.Succeeded)
                {
                    return RedirectToAction(nameof(AdminLogin));
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
        public IActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> AdminLogin(ClientLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(model.Email);
                if (User is not null)
                {
                    var Result = await _userManager.CheckPasswordAsync(User, model.Password);
                    if (Result)
                    {
                        var Login = await _signInManager.PasswordSignInAsync(User, model.Password, model.RememberMe, false);

                        if (Login.Succeeded)
                        {
                            return RedirectToAction("AdminHome", "Admin");
                        }
                    }
                    else ModelState.AddModelError(string.Empty, "Pasword is not correct");
                }
                else ModelState.AddModelError(string.Empty, "Email Doesn't Exist");
            }
            return View(model);
        }

        #endregion

        //////////////////////////////////////////////////////////////////

        //EL ACTIONS MA3A MAIIIIIII//

        // Category/ AllCategory
        #region Category Table
        public IActionResult AllCategory()
        {
            var caregories = _categoryRepo.GetAll();
            return View(caregories);
        }
        #endregion

        #region Add Category 

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Add(category);
                return RedirectToAction(nameof(AllCategory));
            }
            return View(category);
        }

        #endregion

        #region Edit Category //There is No view for it 
        [HttpGet]
        public IActionResult EditCategory(int? id)
        {
            if (id is null)
                return BadRequest();
            var category = _categoryRepo.GetById(id.Value);
            if (category is null)
                return NotFound();
            return View(category);
        }
        [HttpPost]
        public IActionResult EditCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _categoryRepo.Update(category);
                    return RedirectToAction(nameof(AllCategory));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(category);
        }

        #endregion

        #region Delete Category 
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest("Category ID is required.");

            var category = _categoryRepo.GetById(id.Value);
            if (category == null)
                return NotFound("Category not found.");

            _categoryRepo.Delete(category);
            return RedirectToAction(nameof(AllCategory));
        }

        //[HttpPost, ActionName("Delete")]
        //public IActionResult Delete(int id)
        //{
        //    var category = _categoryRepo.GetById(id);
        //    if (category == null)
        //        return NotFound("Category not found.");

        //    try
        //    {
        //        _categoryRepo.Delete(category);
        //        return RedirectToAction(nameof(AllCategory));
        //    }
        //    catch (System.Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, $"Error deleting category: {ex.Message}");
        //        return View(category);
        //    }
        //}

        #endregion

        /////////////////////////////////////////////////////////////////

        #region Products Table
        [HttpGet]
        public IActionResult ProductsPage()
        {
            var Products = _dbContext.Products.Include(p=>p.Seller).ToList();
            return View(Products);
        }


        #endregion

        #region Approve Product
        public IActionResult ApproveProduct([FromRoute] int id)
        {
            var product = _productRepo.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            product.Status = "Approved";
            _productRepo.Update(product);

            return RedirectToAction(nameof(ProductsPage));
        }

        #endregion

        #region Edit Product //There is No view for it 
        [HttpGet]
        public IActionResult EditProduct(int? id)
        {
            if (id is null)
                return BadRequest();
            var product = _productRepo.GetProductById(id.Value);
            if (product is null)
                return NotFound();
            _productRepo.Update(product);
            return View(product);
        }
        [HttpPost]
        public IActionResult EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _productRepo.Update(product);
                    return RedirectToAction(nameof(ProductsPage));
                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(product);
        }

        #endregion

        #region DeleteProduct  
        [HttpGet]
        public IActionResult DeleteProduct(int? id)
        {
            if (id == null)
                return BadRequest("Product ID is required.");

            var product = _productRepo.GetProductById(id.Value);
            if (product == null)
                return NotFound("Product not found.");

            _productRepo.Delete(product);

            return RedirectToAction(nameof(ProductsPage));


        }

        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeleteProduct(int id, Product Product)
        //{
        //    var product = _productRepo.GetProductById(id);
        //    if (product == null)
        //        return NotFound("Product not found.");

        //    try
        //    {
        //        _productRepo.Delete(product);
        //        return RedirectToAction("AllCategory", "Category");
        //    }
        //    catch (System.Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, $"Error deleting product: {ex.Message}");
        //        return View(product);
        //    }
        //}
        #endregion
        /////////////////////////////////////////////////////////////////
        #region Approve Course
        public IActionResult ApproveCourse([FromRoute] int id)
        {
            var course = _courseRepo.GetById(id);
            if (course == null)
            {

                return NotFound();
            }

            course.Status = "Approved";
            _courseRepo.Update(course);

            return RedirectToAction(nameof(CoursesPage));
        }

        #endregion

        #region Courses Table
        public IActionResult CoursesPage()
        {
            var courses = _dbContext.Courses.Include(c => c.Instructor).ToList();
                
            return View(courses);
        }
        #endregion

        //edit courses

        #region Delete Courses
        [HttpGet]
        public IActionResult DeleteCourse(int? id)
        {
            if (id == null)
                return BadRequest("Course ID is required.");

            var course = _courseRepo.GetById(id.Value);
            if (course == null)
                return NotFound("Product not found.");

            _courseRepo.Delete(course);

            return RedirectToAction(nameof(AllCategory));


        }

        #endregion
    }
}
