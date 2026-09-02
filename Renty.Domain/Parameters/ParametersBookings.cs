using Renty.Domain.Models.LookupsTables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Domain.Parameters
{
    /// <summary>
    /// Параметры для фильтрации и пагинации бронирований
    /// </summary>
    /// <params name="Skip">Количество пропускаемых элементов</params>
    /// <params name="PageSize">Количество элементов на странице</params>
    /// <params name="Status">Статус бронирования</params>
    /// <params name="PaymentStatus">Статус оплаты</params>
    /// <params name="FromDate">Дата начала периода</params>
    /// <params name="ToDate">Дата окончания периода</params>
    /// <params name="PropertyId">Идентификатор квартиры</params>
    /// <params name="SortBy">Параметр сортировки</params>
    public sealed class ParametersBookings  
    {
        //база
        public int Skip { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        //фильтрация, статус бронирования, 
        public BookingStatusEnum? Status { get; set; } = null;
        //оплачено ли
        public PaymentStatusEnum? PaymentStatus { get; set; } = null;
        //даты с и до
        public DateTime? FromDate { get; set; } = null;
        public DateTime? ToDate { get; set; } = null;
        //фильтр по конкретной квартире, если нужно
        public Guid? PropertyId { get; set; } = null;
        //ну и как сортировать эту красоту
        public string? SortBy { get; set; } = null;

    }
}
