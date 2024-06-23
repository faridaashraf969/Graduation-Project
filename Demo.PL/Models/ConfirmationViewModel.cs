using Demo.DAL.Entities;

namespace Demo.PL.Models
{
    public class ConfirmationViewModel
    {
        public SessionBid SessionBid { get; set; }
        public Comment Proposal { get; set; }
    }
}
