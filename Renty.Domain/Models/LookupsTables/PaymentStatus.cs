using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Renty.Domain.Models.LookupsTables
{
    //public class PaymentStatus : Lookup
    ///// <summary>
    ///// Pending, Completed, Failed, Refunded, etc.
    ///// </summary>
    //{
    //}
    public enum PaymentStatusEnum
    {
        [Description("В ожидании")]
        Pending = 1,
        [Description("Завершено")]
        Completed = 2,
        [Description("Не удалось")]
        Failed = 3,
        [Description("Возвращено")]
        Refunded = 4
    }
}
