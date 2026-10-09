using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Infrastructure.Services.StripeAPI
{
    public class StripeOptions
    {
        public const string SectionName = "Stripe";
        public string StKey { get; set; } = string.Empty;
        public string PkKey { get; set; } = string.Empty;
        public string WebhookKey { get; set; } = string.Empty;
    }
}
