using Demo.BLL.Interfaces;
using Demo.PL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class ProductController : Controller
    {
        private readonly ICategoryRepo _categoryRepository;
        private readonly IProductRepo _productRepository;
        public ProductController(ICategoryRepo categoryRepository, IProductRepo productRepository) //constructor from product controler with two parameter
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;

        }
        //public IActionResult List()
        //{
        //    var products = _productRepository.products; //get all Products
        //    ProductListViewModel viewModel = new ProductListViewModel();
        //    viewModel.Products = _productRepository.products;
        //    viewModel.CurrentCategory = "productcategory";
        //    return View(viewModel);
        //}
        public IActionResult Cart()
        {
            return View();
        }

        //public IActionResult DeleteProduct()
        //{
        //    return View();
        //}

    }
}
