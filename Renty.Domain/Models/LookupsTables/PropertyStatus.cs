using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Renty.Domain.Models.LookupsTables
{
    //public class PropertyStatus : Lookup
    ///// <summary>
    ///// active, inactive, booked, etc.
    ///// </summary>
    //{
    //}

    public enum PropertyStatusEnum
    {
        [Description("Активен")]
        Active = 1,
        [Description("Неактивен")]
        Inactive = 2,
        [Description("Забронирован")]
        Booked = 3,
        [Description("На обслуживании")]
        UnderMaintenance = 4,
        [Description("В ожидании одобрения")]
        PendingApproval = 5
    }
}
