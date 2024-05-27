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
        [Key]
        public int Id { get; set; }
        [DataType(DataType.Currency)]
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Status { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
