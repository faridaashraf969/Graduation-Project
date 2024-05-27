using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.Helpers;
using Demo.PL.Models.UserLogins;
using Demo.PL.Models.UserRegister;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers.Users
{
    public class SellerController : Controller
    {
        private readonly UserManager<Client> _userManagerClient;
        private readonly SignInManager<Client> _signInManagerClient;
        private readonly IProductRepo _productRepo;

        public SellerController(UserManager<Client> userManager
            , SignInManager<Client> signInManager
            ,IProductRepo productRepo
            )
        {
            this._userManagerClient = userManager;
            this._signInManagerClient = signInManager;
            this._productRepo = productRepo;
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
                var user = new Client()
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    FirstName = model.FName,//Fname for Application User And First name for Modelllll 
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
        public IActionResult Create(Product model)
        {
            model.Status = "Pending";

            if (ModelState.IsValid)
            {
                model.ImageName = DocumentSettings.UploadFille(model.Image, "Images");
                _productRepo.Add(model);
                return RedirectToAction(nameof(ProductList));
            }

            ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors });
            return View(model);
        }
        #endregion


        #region EditProduct
        public IActionResult Edit()
        {
            return View();
        }
        #endregion


        #region DeleteProduct
        public IActionResult Delete()
        {
            return View();
        }
        #endregion


        #region ProductList
        public IActionResult ProductList()
        {
            var products = _productRepo.Getproducts();
            return View(products);
        }
        #endregion

    }
}
