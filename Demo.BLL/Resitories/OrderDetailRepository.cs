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

    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly MvcProjectDbContext _dbContext;

        public OrderDetailRepository(MvcProjectDbContext dbContext) 
        {
            _dbContext = dbContext;
        }



        public void Update(OrderDetail orderDetail)
        {
            _dbContext.OrderDetail.Update(orderDetail);
        }
    }

    public interface IOrderDetailRepository
    {
    }
}