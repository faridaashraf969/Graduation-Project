using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Resitories
{
    public class ProductRepo : IProductRepo
    {
        private readonly MvcProjectDbContext _dbContext;

        public ProductRepo(MvcProjectDbContext dbContext) 
        {
            this._dbContext = dbContext;
        }

        public int Add(Product Product)
        {
            _dbContext.Add(Product);
            return _dbContext.SaveChanges();
        }

        public int Delete(Product product)
        {
            _dbContext.Remove(product);
            return _dbContext.SaveChanges();
            
        }

        public Product GetProductById(int ProductId)
        {
            return _dbContext.Products.FirstOrDefault(p=>p.Id == ProductId);
        }

        public IEnumerable<Product> Getproducts()
        => _dbContext.Products.Include(p => p.Category);

        public void Update(Product product)
        {
            _dbContext.Products.Update(product);
            _dbContext.SaveChanges();
        }
    }
}
