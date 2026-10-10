using Renty.Application.Common;
using Renty.Application.DTOs.Pricing;
using Renty.Domain.Models.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Helpers
{
    public class PriceCalculatorService
    {
        public OperationResult<decimal> Calculate(Property? property, DateOnly checkIn, DateOnly checkOut)
        {
            var nights = checkOut.DayNumber - checkIn.DayNumber;

            if (nights <= 0)
            {
                return OperationResult<decimal>.Fail("Некорректные даты бронирования.");
            }

            if (property == null)
            {
                return OperationResult<decimal>.Fail("Квартира не найдена.");
            }

            var baseTotal = nights * property.PricePerNight;
            decimal discountAmount = 0;

            // скидки из свойства property.Discounts
            var activeDiscounts = property.Discounts?.Where(d => d.IsActive).ToList();

            if (activeDiscounts != null && activeDiscounts.Any())
            {
                decimal maxPercentage = 0;
                int daysUntilCheckIn = checkIn.DayNumber - DateOnly.FromDateTime(DateTime.UtcNow).DayNumber;

                foreach (var discount in activeDiscounts)
                {
                    bool isApplicable = true;

                    if (discount.MaxUses.HasValue && (discount.CurrentUses ?? 0) >= discount.MaxUses.Value)
                    {
                        isApplicable = false;
                    }

                    if (discount.DaysBeforeCheckIn.HasValue && daysUntilCheckIn > discount.DaysBeforeCheckIn.Value)
                    {
                        isApplicable = false;
                    }

                    if (discount.MinNights.HasValue && nights < discount.MinNights.Value)
                    {
                        isApplicable = false;
                    }

                    if (isApplicable && discount.Percentage > maxPercentage)
                    {
                        maxPercentage = discount.Percentage;
                    }
                }

                if (maxPercentage > 0)
                {
                    discountAmount = baseTotal * (maxPercentage / 100m);
                }
            }
            return OperationResult<decimal>.Success(baseTotal - discountAmount);
        }
    }
}
