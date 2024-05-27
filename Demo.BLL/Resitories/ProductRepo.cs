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
        // implement interfce prduct repository
        private readonly MvcProjectDbContext _mVCPhotoCampDbContext;
        public ProductRepo(MvcProjectDbContext mVCPhotoCampDbContext)
        {
            _mVCPhotoCampDbContext = mVCPhotoCampDbContext;
        }

        public IEnumerable<Product> products => _mVCPhotoCampDbContext.Products.Include(c=>c.Category);

        public IEnumerable<Product> preferredProgucts => _mVCPhotoCampDbContext.Products.Where(P => P.IsPreferredProduct).Include(c => c.Category);

        public Product GetProductById(int ProductId) => _mVCPhotoCampDbContext.Products.FirstOrDefault(p => p.Id == ProductId);


        //    private readonly  ICategoryRepository _categoryRepository =new CategoryRepository();
        //    public IEnumerable<Product> products
        //    {
        //        get
        //        {
        //            return new List<Product>
        //            {
        //                new Product {
        //                Name= "Camera",
        //                Price=37000,
        //                ShortDescription="mmmmmmkkkkkkkkk",
        //                LongDescription="jgfjkyghjmmmmmmmmmmmmmm",
        //                Category=_categoryRepository.categories.First(),
        //                ImageUrl="",
        //                InStock=true,
        //                IsPreferredProduct=true,
        //                ImageThumbnailUrl=""
        //                }


        //            };
        //        }
        //    }
        //    public IEnumerable<Product> preferredProgucts { get; set ; }

        //    public Product GetProductById(int ProductId)
        //    {
        //        throw new NotImplementedException();
        //    }
    }
}
