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
        IEnumerable<Product> Getproducts();  //get all product
        Product GetProductById(int ProductId);
        int Add(Product Product);
        void Update(Product product);
        int Delete(Product product);

    }
}
