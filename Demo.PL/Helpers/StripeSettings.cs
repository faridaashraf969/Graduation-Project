namespace Demo.PL.Helpers
{
    public class StripeSettings
    {
        public string ApiKey { get; set; }
        public bool UseSimulatedPayment { get; set; } = true;
    }
}
