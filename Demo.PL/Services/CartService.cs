using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Services
{
    public class CartService
    {
        private readonly MvcProjectDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(MvcProjectDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
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
        private async Task<Cart> GetCourseCartAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User?.Identity?.Name;
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Course)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == userId);

            if (cart == null)
            {
                cart = new Cart { ApplicationUserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        public async Task AddToCartAsync(int productId, int quantity)
        {
            // Check if the cart service is properly initialized
            if (_context == null)
            {
                throw new InvalidOperationException("Cart service is not properly initialized.");
            }

            // Retrieve the cart
            var cart = await GetCartAsync();

            // Check if the cart is null
            if (cart == null)
            {
                throw new InvalidOperationException("Cart is null.");
            }

            // Retrieve the product from the database
            var product = await _context.Products.FindAsync(productId);

            // Check if the product is null
            if (product == null)
            {
                throw new ArgumentException($"Product with ID {productId} does not exist.");
            }

            // Add the product to the cart
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


        public async Task<Cart> GetCartDetailsAsync()
        {
            return await GetCartAsync();
        }
        public async Task<Cart> GetCourseCartDetailsAsync()
        {
            return await GetCourseCartAsync();
        }
        public async Task ClearCartAsync()
        {
            var cart = await GetCartAsync();
            cart.CartItems.Clear();
            await _context.SaveChangesAsync();
        }

        public async Task AddCourseToCartAsync(int courseId, int quantity)
        {
            var cart = await GetCartAsync();
            var course = await _context.Courses.FindAsync(courseId);

            if (course != null)
            {
                var cartItem = new CartItem
                {
                    CourseId = courseId,
                    Course = course,
                    Quantity = quantity,
                    Price = course.Price
                };
                cart.CartItems.Add(cartItem);
                await _context.SaveChangesAsync();
            }
        }


    }
}