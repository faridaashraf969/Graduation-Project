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
        private readonly MvcProjectDbContext _dbConText;
        public CategoryRepo(MvcProjectDbContext mVCPhotoCampDbContext)
        {
            _dbConText = mVCPhotoCampDbContext;
        }

        public IEnumerable<Category> GetAll()
        {
            return _dbConText.Categories.ToList();
        }

    }
}
