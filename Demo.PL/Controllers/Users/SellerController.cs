using Demo.BLL.Interfaces;
using Demo.BLL.Resitories;
using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using Demo.PL.Helpers;
using Demo.PL.Models.UserLogins;
using Demo.PL.Models.UserRegister;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers.Users
{
    public class SellerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManagerClient;
        private readonly SignInManager<ApplicationUser> _signInManagerClient;
        private readonly IProductRepo _productRepo;
        private readonly MvcProjectDbContext _dbContext;

        public SellerController(UserManager<ApplicationUser> userManager
            , SignInManager<ApplicationUser> signInManager
            ,IProductRepo productRepo
            ,MvcProjectDbContext dbContext
            )
        {
            this._userManagerClient = userManager;
            this._signInManagerClient = signInManager;
            this._productRepo = productRepo;
            this._dbContext = dbContext;
        }

        #region Register
        public IActionResult SellerRegister()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SellerRegister(SellerViewModel model)
        {

            if (ModelState.IsValid)//
            {
                var user = new ApplicationUser()
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    FirstName = model.FName, 
                    LastName = model.LName,
                    Role = "Seller",
                    PhoneNumber = model.Phone,
                    SSN = model.SSN,
                    BankAccountNumber = model.BankAccountNumber,
                
                };

                var Result = await _userManagerClient.CreateAsync(user, model.Password);
                if (Result.Succeeded)
                {
                    return RedirectToAction(nameof(SellerLogin));
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
        public IActionResult SellerLogin()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> SellerLogin(ClientLoginViewModel model)
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
                            return RedirectToAction("SellerHome", "Seller");
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
        public IActionResult SellerHome()
        {
            return View();
        }

        #endregion


        #region Store 
        public IActionResult Store()
        {
            return RedirectToAction("Store" , "Home");
        }


        #endregion


        #region AddProduct
        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product model)
        {
            model.Status = "Pending";

            if (ModelState.IsValid)
            {
                model.ImageName = DocumentSettings.UploadFille(model.Image, "Images");

                var user = await _userManagerClient.GetUserAsync(User);
                model.SellerID= user.Id;
                model.Seller = user;

                _productRepo.Add(model);
                return RedirectToAction(nameof(ProductList));
            }

            ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors });
            return View(model);
        }
        #endregion


        #region EditProduct
        public async Task<IActionResult> Edit(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var product = _productRepo.GetProductById(id.Value);
            if (product == null || product.SellerID != (await _userManagerClient.GetUserAsync(User)).Id)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManagerClient.GetUserAsync(User);
                    var existingProduct = await _dbContext.Products.FindAsync(id);

                    if (existingProduct == null || existingProduct.SellerID != user.Id)
                    {
                        return NotFound();
                    }

                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;

                    _dbContext.Update(existingProduct);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ProductList));
            }
            return View(product);
        }

        private bool ProductExists(int id)
        {
            return _dbContext.Products.Any(e => e.Id == id);
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

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult Delete(int id)
        {
            var product = _productRepo.GetProductById(id);
            if (product == null)
                return NotFound("Product not found.");

            try
            {
                _productRepo.Delete(product);
                return RedirectToAction(nameof(ProductList));
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error deleting product: {ex.Message}");
                return View(product);
            }
        }
        #endregion


        #region ProductList 
        public async Task<IActionResult> ProductList()
        {
            var user = await _userManagerClient.GetUserAsync(User);
            var products = _dbContext.Products.Where(p=>p.SellerID== user.Id);
           
            return View(products.ToList());
        }
        #endregion

    }
}
