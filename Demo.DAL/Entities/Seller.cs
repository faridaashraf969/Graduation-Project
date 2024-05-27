using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class Seller : User
    {
            [Required]
            public int SSN { get; set; }
            
            public string BankAccountNumber { get; set; }

        
    }
}
