using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Demo.PL.Services
{
    public class CartService
    {
        private readonly MvcProjectDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartService(MvcProjectDbContext context, IHttpContextAccessor httpContextAccessor,UserManager<ApplicationUser> userManager )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            this._userManager = userManager;
        }
        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        private async Task<Cart> GetCartAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User?.Identity?.Name;

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == userId);

            if (cart == null)
            {
                cart = new Cart { ApplicationUserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }
        //private async Task<Cart> GetCourseCartAsync(string userId)
        //{
        //    var user = await GetUserByIdAsync(userId);
        //    if (user == null)
        //    {
        //        // Handle user not found case
        //        return null;
        //    }

        //    var cart = await _context.Carts
        //                             .Include(c => c.CartItems)
        //                             .ThenInclude(ci=>ci.Course)
        //                             .FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);

        //    if (cart == null)
        //    {
        //        cart = new Cart
        //        { 
        //            ApplicationUserId = userId 

        //        };
        //        _context.Carts.Add(cart);
        //        await _context.SaveChangesAsync();
        //    }

        //    return cart;
        //}

        public async Task AddToCartAsync(int productId, int quantity)
        {
            var cart = await GetCartAsync();

            if (cart == null)
            {
                throw new InvalidOperationException("Cart is null.");
            }

            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                throw new ArgumentException($"Product with ID {productId} does not exist.");
            }

            var cartItem = new CartItem
            {
                ProductId = productId,
                Product = product,
                Quantity = quantity,
                Price = product.Price
            };
            cart.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();
        }


        public async Task AddCourseToCartAsync(int courseId, int quantity ,string userId)
        {
            var cart = await GetCartAsync();

            var cartItem = await _context.CartItems
                                         .FirstOrDefaultAsync(ci => ci.CartId == cart.CartId && ci.CourseId == courseId);
            var course =await _context.Courses.FindAsync(courseId);
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    CourseId = courseId,
                    Quantity = quantity,
                    Price=course.Price
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
                _context.CartItems.Update(cartItem);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<Cart> GetCartDetailsAsync()
        {
            return await GetCartAsync();
        }
        public async Task<Cart> GetCourseCartDetailsAsync()
        {
            return await GetCartAsync();
        }
        public async Task ClearCartAsync()
        {
            var cart = await GetCartAsync();
            cart.CartItems.Clear();
            await _context.SaveChangesAsync();
        }



    }
}