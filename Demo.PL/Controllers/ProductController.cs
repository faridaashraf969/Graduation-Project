using Demo.PL.Services;
using Microsoft.AspNetCore.Mvc;

public class ProductController : Controller
{
    private readonly ProductService _productService;
    private readonly CartService _cartService;

    public ProductController(ProductService productService, CartService cartService)
    {
        _productService = productService;
        _cartService = cartService;
    }

    public IActionResult List()
    {
        var products = _productService.GetAllProducts();
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

    [HttpPost]
    public IActionResult AddToCart(int productId, int quantity)
    {
        var product = _productService.GetProductById(productId);
        if (product == null)
        {
            return NotFound();
        }
        _cartService.AddToCart(product, quantity);
        return RedirectToAction("Index", "Cart");
    }
}
