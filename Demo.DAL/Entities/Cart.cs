using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class Cart
    {
        public List<CartItem> CartItems { get; private set; } = new List<CartItem>();

        //    public void Add(CartItem item)
        //    {
        //        // Add logic here, e.g., increase quantity if item already exists
        //        var existingItem = Items.FirstOrDefault(i => i.Course.CourseId == item.Course.CourseId);
        //        if (existingItem != null)
        //        {
        //            existingItem.Quantity += item.Quantity;
        //        }
        //        else
        //        {
        //            Items.Add(item);
        //        }
        //    }
        //}
    }
}
