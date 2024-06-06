using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using Microsoft.EntityFrameworkCore;
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
            return _context.Products
                .Include(p=>p.Seller)
                .FirstOrDefault(p => p.Id == id);
        }
    }

}
