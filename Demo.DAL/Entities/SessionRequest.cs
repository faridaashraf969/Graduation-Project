using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class SessionRequest
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime SessionDate { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public string Location { get; set; }
        public string Details { get; set; }
        public string SessionType { get; set; }
        public string? PhotographerId { get; set; }
        public ApplicationUser Photographer { get; set; } // Navigational property
        public string? ClientId { get; set; }
        public ApplicationUser Client { get; set; } // Navigational property
        public List<Proposal> Proposals { get; set; } // Navigational property
    }
}
