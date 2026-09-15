using Renty.Domain.Models.LookupsTables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Renty.Domain.Models.Properties
{
    public class Discount
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();

        public Guid PropertyId { get; set; }
        [ForeignKey(nameof(PropertyId))]
        public virtual Property Property { get; set; }

        public DiscountTypeEnum Type { get; set; }

        public decimal Percentage { get; set; } = 20;

        // Таблица условий срабатывания.

        // Для "Новое объявление": максимальное количество использований этой скидки
        public int? MaxUses { get; set; }
        // А это что бы скидку отключать если все были использованы
        public int? CurrentUses { get; set; }

        // Это срок для срабатывания скидки, например что б в "последняя минута" скидка срабатывала только если бронирование сделано за n дней до заезда
        public int? DaysBeforeCheckIn { get; set; }

        // Минимальное колличество ночей бронирования для применения скидки
        public int? MinNights { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
