using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class Proposal
    {
        [Key]
        public int Id { get; set; }
        public string AddtionalDetails { get; set; }
    }
}
