using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Resitories
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly MvcProjectDbContext _mVCPhotoCampDbContext;
        public CategoryRepo(MvcProjectDbContext mVCPhotoCampDbContext)
        {
            _mVCPhotoCampDbContext = mVCPhotoCampDbContext;
        }

        public IEnumerable<Category> categories => _mVCPhotoCampDbContext.Categories;

        //public IEnumerable<Category> categories
        //{
        //    get
        //    {
        //        return new List<Category>
        //        {
        //            new Category { CategoryName="Camera",Description=" All Camera Products"},
        //            new Category { CategoryName="Tripod",Description=" All Tripod Products"},
        //            new Category { CategoryName="Accessories",Description=" All Accessories Products"},
        //            new Category { CategoryName="Light",Description=" All Light Products"}

        //        };
        //    }

        //}

    }
}
