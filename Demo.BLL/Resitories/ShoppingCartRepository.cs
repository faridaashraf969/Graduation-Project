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
   
    
        public class ShoppingCartRepository :  IShoppingCartRepository
        {
            private readonly MvcProjectDbContext _dbContext;

            public ShoppingCartRepository(MvcProjectDbContext dbContext)
            {
                _dbContext = dbContext;
            }
            public void Update(ShoppingCart obj)
            {
                _dbContext.ShoppingCart.Update(obj);
            }
        }
    
}

