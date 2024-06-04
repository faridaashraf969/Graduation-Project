using Demo.DAL.Entities;
using System.Collections.Generic;

namespace Demo.PL.Models
{
    public class CheckOutViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        // Add any other properties you need for the checkout process
    }
}
