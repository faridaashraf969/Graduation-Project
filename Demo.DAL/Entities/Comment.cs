using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; } // Navigational property
        public int SessionBidId { get; set; }
        public SessionBid SessionBid { get; set; } // Navigational property
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

    }
}
