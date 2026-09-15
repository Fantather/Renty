using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Domain.Models.LookupsTables
{
    public enum DiscountTypeEnum
    {
        /// <summary>
        /// Акция «Новое объявление» (скидка на первые бронирования)
        /// </summary>
        NewListingPromo = 1,

        /// <summary>
        /// Скидка последней минуты (менее чем за N дней до прибытия)
        /// </summary>
        LastMinute = 2,

        /// <summary>
        /// Недельная скидка (от 7 ночей)
        /// </summary>
        Weekly = 3,

        /// <summary>
        /// Месячная скидка (от 28 ночей)
        /// </summary>
        Monthly = 4
    }
}
