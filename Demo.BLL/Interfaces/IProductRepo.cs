using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IProductRepo
    {
        // CRUD Operation
        IEnumerable<Product> products { get; }  //getb all product
        IEnumerable<Product> preferredProgucts { get; } // get preferredproduct only at home page 
        Product GetProductById(int ProductId);
    }
}
