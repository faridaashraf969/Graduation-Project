using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface ICategoryRepo
    {
        // CRUD operation
        // IEnumerable<Category> categories { get; } // get all ccategories 
        IEnumerable<Category> GetAll();
        Category GetById(int id);
        int Add(Category category);
        int Update(Category category);
        int Delete(Category category);
    }
}
