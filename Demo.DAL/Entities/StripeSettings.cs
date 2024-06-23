using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Entities
{
    public class StripeSettingss
    {
        public string PublishableKey { get; set; }
        public string SecretKey { get; set; }

        public bool IsPaid { get; set; } = true;
    }
}
