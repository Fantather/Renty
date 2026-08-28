using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Renty.Domain.Models.LookupsTables
{
    //public class BookingStatus : Lookup
    //{
    //}

    public enum BookingStatusEnum
    {
        [Description("В ожидании")]
        Pending = 1,
        [Description("Подтверждено")]
        Confirmed = 2,
        [Description("Отменено")]
        Cancelled = 3,
        [Description("Завершено")]
        Completed = 4
    }
}

