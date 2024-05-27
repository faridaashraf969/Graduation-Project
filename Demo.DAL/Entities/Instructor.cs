using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class Instructor : User
    {
        [Required]
        [Key]
        public int SSN { get; set; }

        public int YearsOfExperience { get; set; }

        public string BankAccountNumber { get; set; }

    }
}
