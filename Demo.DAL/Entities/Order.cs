using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class Order
    {
         

        [Key]
        public int OrderNumber { get; set; }
        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }
     
      
        public DateTime DeliveryDate { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public int CustomerID { get; set; }

        [RegularExpression("^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
         ErrorMessage = "Address must be Like 123-Street-City-Country")]
        public string ShippingAddress { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser User { get; set; }

        public ICollection<Course> Courses { get; set; } = new List<Course>(); // Add this property





    }
}
