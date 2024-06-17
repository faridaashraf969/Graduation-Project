using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.PL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class ProductController : Controller
{
    private readonly ProductService _productService;
    private readonly CartService _cartService;
    private readonly MvcProjectDbContext _dbContext;

    public ProductController(ProductService productService, CartService cartService ,MvcProjectDbContext dbContext )
    {
        _productService = productService;
        _cartService = cartService;
        this._dbContext = dbContext;
    }

    public IActionResult List()
    {
        var products = _dbContext.Products.Include(p=>p.Category).Where(p => p.Status == "Approved");

        return View(products);
    }
    [HttpPost]
    public IActionResult List(string category)
    {
        var Products = _dbContext.Products.Include(p => p.Category);
        var products =Products.Where(p => p.Category.Name.ToUpper().Contains(category.ToUpper()));
        return View(products);
    }

    public IActionResult Details(int id)
    {
        var product = _productService.GetProductById(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    //[HttpPost]
    //public IActionResult AddToCart(int productId, int quantity)
    //{
    //    var product = _productService.GetProductById(productId);
    //    if (product == null)
    //    {
    //        return NotFound();
    //    }
    //    _cartService.AddToCart(product, quantity);
    //    return RedirectToAction("Index", "Cart");
    //}
}
