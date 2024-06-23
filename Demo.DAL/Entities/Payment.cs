using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int SessionBidId { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        // Other properties...
    }
}
