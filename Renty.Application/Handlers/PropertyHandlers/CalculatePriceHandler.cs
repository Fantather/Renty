using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.Pricing;
using Renty.Application.Queries;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.Properties;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Renty.Application.Handlers.PropertyHandlers
{
    public class CalculatePriceQueryHandler : IRequestHandler<CalculatePriceQuery, OperationResult<PriceCalculationResponse>>
    {
        private readonly IPropertyRepository _propertyRepository;


        public CalculatePriceQueryHandler(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        public async Task<OperationResult<PriceCalculationResponse>> Handle(CalculatePriceQuery request, CancellationToken cancellationToken)
        {
            var nights = request.CheckOut.DayNumber - request.CheckIn.DayNumber;

            if (nights <= 0)
            {
                return OperationResult<PriceCalculationResponse>.Fail("Некорректные даты бронирования.");
            }

            // квартира со скидками и так приедет
            var property = await _propertyRepository.GetPropertyWithDetailsAsync(request.Slug, cancellationToken);

            if (property == null)
            {
                return OperationResult<PriceCalculationResponse>.Fail("Квартира не найдена.");
            }

            var baseTotal = nights * property.PricePerNight;
            decimal discountAmount = 0;

            // скидки из свойства property.Discounts
            var activeDiscounts = property.Discounts?.Where(d => d.IsActive).ToList();

            if (activeDiscounts != null && activeDiscounts.Any())
            {
                decimal maxPercentage = 0;
                int daysUntilCheckIn = request.CheckIn.DayNumber - DateOnly.FromDateTime(DateTime.UtcNow).DayNumber;

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

            var response = new PriceCalculationResponse
            {
                Nights = nights,
                PricePerNight = property.PricePerNight,
                BaseTotal = baseTotal,
                DiscountAmount = discountAmount,
                FinalTotal = baseTotal - discountAmount
            };

            return OperationResult<PriceCalculationResponse>.Success(response);
        }
    }
}