using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class SessionBid
    {
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime SessionDate { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Required]
        public string Location { get; set; }
        public string Details { get; set; }
        [Required]
        public string SessionType { get; set; }
        public string PhotographerId { get; set; }
        public ApplicationUser Photographer { get; set; } // Navigational property
        public string ClientId { get; set; }
        public ApplicationUser Client { get; set; } // Navigational property
        public ICollection<Comment> Comments { get; set; } // Navigational property
    }
}
