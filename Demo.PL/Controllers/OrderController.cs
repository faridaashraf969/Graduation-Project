﻿using Demo.DAL.Entities;
using Demo.PL.Helpers;
using Demo.PL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Demo.DAL.Contexts;

namespace Demo.PL.Controllers
{
    public class OrderController : Controller
    {
        private readonly MvcProjectDbContext _context;
        private readonly CartService _cartService;
        private readonly StripeSettings _stripeSettings;

        public OrderController(MvcProjectDbContext context, CartService cartService, IOptions<StripeSettings> stripeSettings)
        {
            _context = context;
            _cartService = cartService;
            _stripeSettings = stripeSettings.Value;
        }

        public IActionResult OrderSummary(int orderId)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .FirstOrDefault(o => o.OrderNumber == orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var cart = await _cartService.GetCartDetailsAsync();

            if (cart.CartItems.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }

            var order = new Order
            {
                UserId = cart.ApplicationUserId,
                OrderDate = DateTime.Now,
                Status = "Pending",
                TotalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.Price),
                OrderItems = new List<OrderItem>() // Initialize the OrderItems list
            };

            foreach (var cartItem in cart.CartItems)
            {
                var orderItem = new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Price
                };
                order.OrderItems.Add(orderItem);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            await _cartService.ClearCartAsync();

            return RedirectToAction("OrderSummary", new { orderId = order.OrderNumber });
        }
        [HttpPost]
        public async Task<IActionResult> CourseCheckout(string userId)
        {
            var cart = await _cartService.GetCourseCartDetailsAsync(userId);

            if (cart.CartItems.Count == 0)
            {
                return RedirectToAction("CourseIndex", "Cart");
            }

            var order = new Order
            {
                UserId = cart.ApplicationUserId,
                OrderDate = DateTime.Now,
                Status = "Pending",
                TotalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.Price),
                OrderItems = new List<OrderItem>() // Initialize the OrderItems list
            };

            foreach (var cartItem in cart.CartItems)
            {
                var orderItem = new OrderItem
                {
                    ProductId = cartItem.CourseId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Price
                };
                order.OrderItems.Add(orderItem);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            await _cartService.ClearCartAsync();

            return RedirectToAction("CourseOrderSummary", new { orderId = order.OrderNumber });
        }
        public IActionResult CourseOrderSummary(int orderId)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Course)
                .Include(o => o.User)
                .FirstOrDefault(o => o.OrderNumber == orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }


        public IActionResult PaymentConfirmation(int orderId)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Course)
                .Include(o => o.User)
                .FirstOrDefault(o => o.OrderNumber == orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(int orderId)
        {
            //var order = await _context.Orders
            //    .Include(o => o.OrderItems)
            //    .ThenInclude(oi => oi.Product)
            //    .Include(o => o.User)
            //    .FirstOrDefaultAsync(o => o.OrderNumber == orderId);

            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Course)
                .Include(o => o.User)
                .FirstOrDefault(o => o.OrderNumber == orderId);

            if (order == null)
            {
                return NotFound();
            }

            if (_stripeSettings.UseSimulatedPayment)
            {
                // Simulate payment process
                order.Status = "Paid";
                await _context.SaveChangesAsync();
                return RedirectToAction("PaymentConfirmation", new { orderId = order.OrderNumber });
            }
            else
            {
                // Process payment using Stripe
                var domain = "https://yourdomain.com";
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = $"{domain}/Order/PaymentConfirmation?orderId={orderId}",
                    CancelUrl = $"{domain}/Order/OrderSummary?orderId={orderId}",
                };

                foreach (var item in order.OrderItems)
                {
                    options.LineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Name,
                            },
                        },
                        Quantity = item.Quantity,
                    });
                }

                var service = new SessionService();
                Session session = service.Create(options);

                order.Status = "Processing";
                await _context.SaveChangesAsync();

                return Redirect(session.Url);
            }
        }
    }
}