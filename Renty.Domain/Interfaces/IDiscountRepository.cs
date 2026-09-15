using Renty.Domain.Models.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Domain.Interfaces
{
    public interface IDiscountRepository : IGenericRepository<Discount>
    {
        /// <summary>
        /// Получает список всех активных скидок для указанной квартиры.
        /// </summary>
        /// <param name="propertyId">Идентификатор квартиры.</param>
        /// <param name="ct">Токен отмены.</param>
        /// <returns>Список активных скидок.</returns>
        Task<IEnumerable<Discount>> GetActiveByPropertyIdAsync(Guid propertyId, CancellationToken ct = default);
    }
}
