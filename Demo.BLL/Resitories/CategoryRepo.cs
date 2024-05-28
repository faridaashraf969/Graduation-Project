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
    public class CategoryRepo : ICategoryRepo
    {
        private readonly MvcProjectDbContext _projectDbContext;
        // create instructor to open coection with DBcontext while only that crude operation
        public CategoryRepo(MvcProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }


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
        public int Add(Category category)
        {
            _projectDbContext.Add(category);
            return _projectDbContext.SaveChanges();
        }

        public int Delete(Category category)
        {
            _projectDbContext.Add(category);
            return _projectDbContext.SaveChanges();
        }

        public IEnumerable<Category> GetAll()
        {
            return _projectDbContext.Categories.ToList();
        }

        public Category GetById(int id)
        {
            return _projectDbContext.Categories.Find(id);
        }

        public int Update(Category category)
        {
            _projectDbContext.Update(category);
            return _projectDbContext.SaveChanges();
        }
    }
}
