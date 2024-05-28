using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.Models.UserLogins;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers.Users
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IProductRepo _productRepo;

        public AdminController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
            ,IProductRepo productRepo) 
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._productRepo = productRepo;
        }
        #region Admin Home
        public IActionResult AdminHome() //Admin Page (Mai Salah 3la Telegram)
        {
            return View();
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
                            return RedirectToAction("AdminHome", "Home");
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
        
        //EL ACTIONS MA3A MAIIIIIII
        #region Add Category 

        #endregion

        #region Edit Category 

        #endregion

        #region Delete Category 

        #endregion

        /////////////////////////////////////////////////////////////////

        #region Products Table
        [HttpGet]
        public IActionResult ProductsPage()
        {
            var Products = _productRepo.Getproducts();
            return View(Products);
        }


        #endregion

        #region Approve Product
        public IActionResult Approve([FromRoute] int id)
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

        #region Edit Product

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

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _productRepo.GetProductById(id);
            if (product == null)
                return NotFound("Product not found.");

            try
            {
                _productRepo.Delete(product);
                return RedirectToAction("AllCategory", "Category");
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error deleting product: {ex.Message}");
                return View(product);
            }
        }
        #endregion
        /////////////////////////////////////////////////////////////////
    }
}
