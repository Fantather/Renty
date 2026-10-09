using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Domain.ServiceModels.Stripe
{
    
    public class BalanceDto
    {
        // Доступная сумма в центах
        public long AvailableInCents { get; set; }
        // Сумма в ожидании зачисления в центах
        public long PendingInCents { get; set; }
        public string Currency { get; set; } = "usd";
    }
}
