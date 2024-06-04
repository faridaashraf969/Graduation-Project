using Bulky.DataAcess.Repository.IRepository;
using Bulky.Models;
using BulkyAcess.DataAcess.Data;
using Demo.DAL.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAcess.Repository
{

    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly MvcProjectDbContext _dbContext;

        public OrderDetailRepository(MvcProjectDbContext dbContext) : base( dbContext)
        {
            _dbContext = dbContext;
        }



        public void Update(OrderDetail orderDetail)
        {
            _dbContext.OrderDetails.Update(orderDetail);
        }
    }
}