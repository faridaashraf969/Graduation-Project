using Demo.DAL.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

public class OrderController : Controller
{
    private readonly MvcProjectDbContext _context;

    public OrderController(MvcProjectDbContext context)
    {
        _context = context;
    }

    public IActionResult OrderSummary(int orderId)
    {
        var order = _context.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.User)

            .FirstOrDefault(o => o.OrderNumber == orderId);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }
    [HttpPost]
    public IActionResult ProcessPayment(int orderId, string cardNumber, string expiryDate, string cvv)
    {
        // Simulate payment processing
        bool paymentSuccessful = ProcessPayment(cardNumber, expiryDate, cvv);

        if (paymentSuccessful)
        {
            // Update order status to paid
            var order = _context.Orders.FirstOrDefault(o => o.OrderNumber == orderId);
            if (order != null)
            {
                order.Status = "Paid";
                _context.SaveChanges();
            }

            // Redirect to confirmation page
            return RedirectToAction(nameof(PaymentConfirmation), new { orderId = orderId });
        }
        else
        {
            // Handle payment failure
            ModelState.AddModelError(string.Empty, "Payment processing failed. Please try again.");
            return RedirectToAction("OrderSummary", new { orderId = orderId });
        }
    }
    private bool ProcessPayment(string cardNumber, string expiryDate, string cvv)
    {
        // Placeholder for actual payment processing logic
        // Integrate with a payment gateway here
        return true; // Simulate successful payment
    }
    
    public IActionResult PaymentConfirmation(int orderId)
    {
        var order = _context.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.User)
            .FirstOrDefault(o => o.OrderNumber == orderId);

        if (order == null)
        {
            return NotFound();
        }

        return View("PaymentConfirmation", order);
    }

}
