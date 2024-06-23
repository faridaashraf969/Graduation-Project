namespace Demo.PL.Models
{
    public class PaymentViewModel
    {
        public int SessionBidId { get; set; }
        public decimal Amount { get; set; }
        public string StripeToken { get; set; }
        public string PublishableKey { get; set; }
        public int proposalId { get; set; }
    }
}
