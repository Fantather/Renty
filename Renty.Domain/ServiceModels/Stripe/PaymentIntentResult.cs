using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Domain.ServiceModels.Stripe
{
    public class PaymentIntentResult
    {
        public string PaymentIntentId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
        public string Status { get; set; } = string.Empty;
    }
}
