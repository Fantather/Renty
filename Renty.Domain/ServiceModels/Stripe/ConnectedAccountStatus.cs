using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Domain.ServiceModels.Stripe
{
    public class ConnectedAccountStatus
    {
        public bool CanReceiveTransfers { get; set; }
        public bool HasOutstandingRequirements { get; set; }
    }
}
