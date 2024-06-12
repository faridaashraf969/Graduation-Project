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


        public CartController(CartService cartService,UserManager<ApplicationUser> userManager, MvcProjectDbContext dbContext)
        {
            _cartService = cartService;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        private async Task<string> GetUserIdAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new InvalidOperationException("User is not authenticated.");
            }
            return user.Id;
        }

        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetCartDetailsAsync();
            return View(cart);
        }

        public async Task<IActionResult> AddToCart(int productId, int quantity = 1 )
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
            var cartItems = await _dbContext.CartItems
           .Include(ci => ci.Product) 
           .ToListAsync();
            if (cartItems == null || !cartItems.Any())
            {
                return NotFound();
            }
            return View(cartItems);
        }

    }
}