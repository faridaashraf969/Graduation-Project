using Bulky.Models;
using Bulky.Models.ViewModel;
using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using Demo.PL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        //private readonly OrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MvcProjectDbContext _dbContext;


        public CartController(CartService cartService,/* OrderService orderService*/ UserManager<ApplicationUser> userManager, MvcProjectDbContext dbContext)
        {
            _cartService = cartService;
            //_orderService = orderService;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetCartDetailsAsync();
            return View(cart);
        }
        public async Task<IActionResult> CourseIndex()
        {
            var cart = await _cartService.GetCourseCartDetailsAsync( );
            return View(cart);
        }

        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            await _cartService.AddToCartAsync(productId, quantity);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ClearCart()
        {
            await _cartService.ClearCartAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> CheckoutAsync()
        {
            // Retrieve the cart items from the database or wherever they're stored
            var cartItems = await _dbContext.CartItems
           .Include(ci => ci.Product) // Include the Product entity
           .ToListAsync();// Modify this as needed

            if (cartItems == null || !cartItems.Any())
            {
                return NotFound(); // Handle the case where cart items are not found
            }

            return View(cartItems); // Pass the cart items to the view
        }
        /////////////////////////////////////////////////////////////////////////////////////
        public async Task<IActionResult> AddCourseToCart(int courseId, int quantity = 1)
        {
            var userid = _userManager.GetUserId(User);

            // Ensure the user is not null
            if (userid == null)
            {
                // Handle the case where the user is not authenticated or not found
                return RedirectToAction("Register", "Account");
            }

            // Get the user ID


            // Add the course to the cart
            await _cartService.AddCourseToCartAsync(courseId, quantity, userid);

            // Redirect to the cart page
            return RedirectToAction("Index", "Cart");
        }

    }
}