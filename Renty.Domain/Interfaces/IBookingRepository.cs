using Renty.Domain.Models.LookupsTables;
using Renty.Domain.Models.Orders;
using Renty.Domain.Parameters;
using System;
using System.Collections.Generic;
using System.Text;
using Renty.Domain.Parameters;

namespace Renty.Domain.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        /// <summary>
        /// Получить все бронирования пользователя по его идентификатору
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="ct">Токен отмены</param>
        /// <param name="param">Параметры фильтрации и пагинации</param>
        /// <returns>Список бронирований</returns>
        Task<IEnumerable<Booking>> GetUserBookingsAsync(Guid userId, ParametersBookings? param = null, CancellationToken ct = default);
        /// <summary>
        /// Получить все бронирования пользователя по его username
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <param name="ct">Токен отмены</param>
        /// <param name="param">Параметры фильтрации и пагинации</param>
        /// <returns>Список бронирований</returns>
        Task<IEnumerable<Booking>> GetUserBookingsAsync(string username, ParametersBookings? param = null, CancellationToken ct = default);

        /// <summary>
        /// Получить все бронирования для конкретной квартиры по её идентификатору, для владельца квартиры
        /// </summary>
        /// <remarks>
        /// <b>ДЛЯ ВЛАДЕЛЬЦА. Проверяйте права доступа!</b>
        /// </remarks>
        /// <param name="propertyId">Идентификатор квартиры</param>
        /// <param name="ct">Токен отмены</param>
        /// <param name="ownerId">Идентификатор владельца квартиры</param>
        /// <param name="param">Параметры фильтрации и пагинации</param>
        /// <returns>Список бронирований</returns>

        Task<IEnumerable<Booking>> GetPropertyBookingsAsync(Guid propertyId, Guid ownerId, ParametersBookings? param = null, CancellationToken ct = default);
        /// <summary>
        /// Получить список всех актуальных бронирований для календаря (будущие и текущие).
        /// Отмененные и прошедшие бронирования исключаются.
        /// </summary>
        /// <param name="propertyId">Идентификатор квартиры</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Список бронирований для маппинга в сервисе</returns>
        Task<IEnumerable<Booking>> GetActiveBookingsForCalendarAsync(Guid propertyId, CancellationToken ct = default);

        /// <summary>
        /// Проверяет, доступен ли диапазон дат для бронирования конкретной квартиры
        /// </summary>
        /// <param name="propertyId">Идентификатор квартиры</param>
        /// <param name="checkIn">Дата заезда</param>
        /// <param name="checkOut">Дата выезда</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns><c>true</c>, если диапазон дат доступен для бронирования, иначе <c>false</c></returns>
        Task<bool> IsDateRangeAvailableAsync(Guid propertyId, DateTime checkIn, DateTime checkOut, CancellationToken ct = default);

        /// <summary>
        /// Изменяет статус бронирования
        /// </summary>
        /// <param name="bookingId">Идентификатор бронирования</param>
        /// <param name="newStatus">Новый статус бронирования</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns><c>true</c>, если статус был успешно изменен, иначе <c>false</c></returns>
        Task<bool> ChangeBookingStatusAsync(Guid bookingId, BookingStatusEnum newStatus, CancellationToken ct = default);

        /// <summary>
        /// Изменяет статус оплаты бронирования
        /// </summary>
        /// <param name="bookingId">Идентификатор бронирования</param>
        /// <param name="newStatus">Новый статус оплаты</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns><c>true</c>, если статус оплаты был успешно изменен, иначе <c>false</c></returns>
        Task<bool> ChangePaymentStatusAsync(Guid bookingId, PaymentStatusEnum newStatus, CancellationToken ct = default);
    }
}
