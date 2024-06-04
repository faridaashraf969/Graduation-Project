
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAcess.Repository.IRepository
{
    public interface IOrderDetailRepository
    {
        void Update(Models.OrderDetail orderDetail);
    }
}