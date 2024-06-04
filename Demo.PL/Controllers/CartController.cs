using Bulky.Models;
using Bulky.Models.ViewModel;
using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

public class CartController : Controller
{
    private readonly CartService _cartService;
    private readonly OrderService _orderService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly MvcProjectDbContext _dbContext;

    public CartController(CartService cartService, OrderService orderService, UserManager<ApplicationUser> userManager, MvcProjectDbContext dbContext)
    {
        _cartService = cartService;
        _orderService = orderService;
        _userManager = userManager;
        this._dbContext = dbContext;
    }

    //public IActionResult Index()
    //{
    //    var cartItems = _cartService.GetCartItems();
    //    return View(cartItems);
    //}

    //public IActionResult Checkout()
    //{
    //    var cartItems = _cartService.GetCartItems();
    //    return View(cartItems);
    //}

    //[HttpPost]
    //[Authorize]
    //public async Task<IActionResult> CheckoutConfirm(string shippingAddress, string billingAddress)
    //{
    //    var user = await _userManager.GetUserAsync(User);
    //    if (user == null)
    //    {
    //        return Challenge();
    //    }

    //    var cartItems = _cartService.GetCartItems();
    //    if (!cartItems.Any())
    //    {
    //        return RedirectToAction("Index");
    //    }

    //    var order = _orderService.CreateOrder(user.Id, cartItems, shippingAddress, billingAddress);

    //    // Clear the cart after creating the order
    //    _cartService.ClearCart();

    //    // Redirect to Order Summary for review
    //    return RedirectToAction("OrderSummary", "Order", new { orderId = order.OrderNumber });
    //}
    //////////////////////////////////////////////////////////////////////////



    [BindProperty]
    public ShoppingCartVM ShoppingCartVM { get; set; }


    public IActionResult Index()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        ShoppingCartVM = new()
        {
            ShoppingCartList = _dbContext.ShoppingCart.GetAll(x => x.ApplicationUserId == UserId, includeProperties: "Product"),
            OrderHeader = new()

        };
        foreach (var cart in ShoppingCartVM.ShoppingCartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart);
            ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);

        }

        return View(ShoppingCartVM);
    }
    public IActionResult Summary()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

        ShoppingCartVM = new()
        {
            ShoppingCartList = _dbContext.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
            includeProperties: "Product"),
            OrderHeader = new()
        };

        ShoppingCartVM.OrderHeader.ApplicationUser = _dbContext.ApplicationUser.Get(u => u.Id == userId);

        ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.FirstName;
        ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
        ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
        ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
        ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
        

        foreach (var cart in ShoppingCartVM.ShoppingCartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart);
            ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
        }
        return View(ShoppingCartVM);
    }




    [HttpPost]
    [ActionName("Summary")]
    public IActionResult SummaryPost()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

        ShoppingCartVM.ShoppingCartList = _dbContext.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product");

        ApplicationUser applicationUser = _dbContext.ApplicationUser.Get(u => u.Id == userId);

        ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
        ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

        foreach (var cart in ShoppingCartVM.ShoppingCartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart);
            ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
        }
        if (applicationUser.CompanyId ==0)
        {
            // regular user
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
        }
        else
        {
            //company user
            ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
            ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
        }
        _dbContext.OrderHeader.Add(ShoppingCartVM.OrderHeader);
        _dbContext.SaveChanges();

        foreach (var cart in ShoppingCartVM.ShoppingCartList)
        {
            OrderDetail orderDetail = new()
            {
                ProductId = cart.ProductId,
                Count = cart.Count,
                Price = cart.Price,
                OrderHeaderId = ShoppingCartVM.OrderHeader.Id

            };
            _dbContext.OrderDetail.Add(orderDetail);
            _dbContext.SaveChanges();
        }

        if (applicationUser.Id.GetValueOrDefault() == 0)
        {
            var domain = "http://localhost:5273/";
            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                CancelUrl = domain + "customer/cart/index",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };
            foreach (var item in ShoppingCartVM.ShoppingCartList)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100), // $20.50 => 2050
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name
                        }
                    },
                    Quantity = item.Count
                };
                options.LineItems.Add(sessionLineItem);
            }


            var service = new SessionService();
            Session session = service.Create(options);
            _dbContext.OrderHeader.UpdateStripePaymentID(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
            _dbContext.SaveChanges();
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

        }

        return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartVM.OrderHeader.Id });

    }




    public IActionResult OrderConfirmation(int id)
    {
        OrderHeader orderHeader = _dbContext.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");
        if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
        {
            //this is an order by customer

            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                _dbContext.OrderHeader.UpdateStripePaymentID(id, session.Id, session.PaymentIntentId);
                _dbContext.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                _dbContext.SaveChanges();
            }


        }

        List<ShoppingCart> shoppingCarts = _dbContext.ShoppingCart.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();

        _dbContext.ShoppingCart.RemoveRange(shoppingCarts);
        _dbContext.SaveChanges();

        return View(id);
    

    }
}


