using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.Balance
{
    public class GetBalanceDto
    {
        // Доступная сумма
        public decimal Available { get; set; }
        // Сумма в ожидании зачисления
        public decimal Pending { get; set; }
        // Общая сумма
        public decimal Total { get; set; }
        // Валюта 
        public string Currency { get; set; } = "USD";
    }
}
