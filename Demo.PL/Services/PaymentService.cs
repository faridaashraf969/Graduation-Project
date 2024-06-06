using System;

namespace Demo.PL.Services
{
    using Stripe;
    using Microsoft.Extensions.Options;
    using System.Threading.Tasks;
    using Demo.PL.Helpers;

    public class PaymentService
    {
        private readonly StripeSettings _stripeSettings;

        public PaymentService(IOptions<StripeSettings> stripeSettings)
        {
            _stripeSettings = stripeSettings.Value;
            StripeConfiguration.ApiKey = _stripeSettings.ApiKey;
        }

        public async Task<bool> ProcessPayment(string token, decimal amount)
        {
            var options = new ChargeCreateOptions
            {
                Amount = (long)(amount * 100), // Stripe expects the amount in cents
                Currency = "Egp",
                Description = "Order Payment",
                Source = token
            };
            var service = new ChargeService();
            Charge charge = await service.CreateAsync(options);
            return charge.Status == "succeeded";
        }
    }

}
