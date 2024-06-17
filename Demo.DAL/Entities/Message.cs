using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; } // Navigational property
        public string ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; } // Navigational property
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        
            
    }
}
