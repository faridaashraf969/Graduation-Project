using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class Photographer : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Role { get; set; }
        public string Address { get; set; }
        public string SSN { get; set; }
        public string Specialty { get; set; }
        public string PortofiloUrl { get; set; }
        public string BankAccountNumber { get; set; }
        public string AccountStatus { get; set; }
        public string Feedback { get; set; }
        public bool IsActive { get; set; }
    }
}
