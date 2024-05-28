using Demo.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

public class CartController : Controller
{
    private readonly CartService _cartService;
    private readonly OrderService _orderService;
    private readonly UserManager<ApplicationUser> _userManager;

    public CartController(CartService cartService, OrderService orderService, UserManager<ApplicationUser> userManager)
    {
        _cartService = cartService;
        _orderService = orderService;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        var cartItems = _cartService.GetCartItems();
        return View(cartItems);
    }

    public IActionResult Checkout()
    {
        var cartItems = _cartService.GetCartItems();
        return View(cartItems);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CheckoutConfirm(string shippingAddress, string billingAddress)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Challenge();
        }

        var cartItems = _cartService.GetCartItems();
        if (!cartItems.Any())
        {
            return RedirectToAction("Index");
        }

        var order = _orderService.CreateOrder(user.Id, cartItems, shippingAddress, billingAddress);

        // Clear the cart after creating the order
        _cartService.ClearCart();

        // Redirect to Order Summary for review
        return RedirectToAction("OrderSummary", "Order", new { orderId = order.OrderNumber });
    }
}
