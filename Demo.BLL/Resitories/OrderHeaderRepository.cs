
//using Bulky.DataAcess.Repository.IRepository;
//using Demo.DAL.Contexts;
//using Demo.BLL.Interfaces;

//using Demo.DAL.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//namespace Demo.BLL.Resitories
//{
//    public class OrderHeaderRepository : IOrderHeaderRepository
//    {
//        private readonly MvcProjectDbContext _dbContext;

//        public OrderHeaderRepository (MvcProjectDbContext dbContext) 
//        {
//            _dbContext = dbContext;
//        }


//        public void Update(OrderHeader orderHeader)
//        {
//            _dbContext.OrderHeader.Update(orderHeader);
//        }
//        public void UpdateStatus(int id, string orderStatus, string paymentStatus = null)
//        {
//            var orderFromDb = _dbContext.OrderHeader.FirstOrDefault(u => u.Id == id);
//            if (orderFromDb != null)
//            {
//                orderFromDb.OrderStatus = orderStatus;
//                if (!string.IsNullOrEmpty(paymentStatus))
//                {
//                    orderFromDb.PaymentStatus = paymentStatus;
//                }
//            }
//        }

//        public void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId)
//        {
//            var orderFromDb = _dbContext.OrderHeader.FirstOrDefault(u => u.Id == id);
//            if (!string.IsNullOrEmpty(sessionId))
//            {
//                orderFromDb.SessionId = sessionId;
//            }
//            if (!string.IsNullOrEmpty(paymentIntentId))
//            {
//                orderFromDb.PaymentIntentId = paymentIntentId;
//                orderFromDb.PaymentDate = DateTime.Now;
//            }
//        }

//        void IOrderHeaderRepository.Update(OrderHeader orderHeader)
//        {
//            throw new NotImplementedException();
//        }

//        void IOrderHeaderRepository.UpdateStatus(int id, string orderStatus, string paymentStatus)
//        {
//            throw new NotImplementedException();
//        }

//        void IOrderHeaderRepository.UpdateStripePaymentID(int id, string sessionId, string paymentIntentId)
//        {
//            throw new NotImplementedException();
//        }
//    }


//}
