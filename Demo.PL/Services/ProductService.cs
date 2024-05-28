using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Demo.PL.Services
{
    public class ProductService
    {
        private readonly MvcProjectDbContext _context;

        public ProductService(MvcProjectDbContext context)
        {
            _context = context;
        }

        public List<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public Product GetProductById(int id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }
    }

}
