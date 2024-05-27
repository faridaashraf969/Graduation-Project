using Demo.DAL.Entities;
using System.Collections.Generic;

namespace Demo.PL.Models
{
    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public string CurrentCategory { get; set; }
    }
}
