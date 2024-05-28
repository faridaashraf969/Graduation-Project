using Demo.DAL.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            .Include(o => o.ApplicationUser)
            .FirstOrDefault(o => o.OrderNumber == orderId);

        if (order == null)
        {
            return NotFound();
        }

        return View(order);
    }
}
