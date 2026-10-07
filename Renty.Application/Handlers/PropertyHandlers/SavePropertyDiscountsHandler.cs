using MediatR;
using Renty.Application.Commands.PropertyCommands;
using Renty.Application.Common;
using Renty.Application.Helpers;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.LookupsTables;
using Renty.Domain.Models.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.PropertyHandlers
{
    public class SavePropertyDiscountsHandler : IRequestHandler<SavePropertyDiscountsCommand, OperationResult<Unit>>
    {
        private readonly OwnedPropertyService _ownedPropertyService;
        private readonly IDiscountRepository _discountRepository;
        public SavePropertyDiscountsHandler(
            OwnedPropertyService ownedPropertyService,
            IDiscountRepository discountRepository)
        {
            _ownedPropertyService = ownedPropertyService;
            _discountRepository = discountRepository;
        }
        public async Task<OperationResult<Unit>> Handle(SavePropertyDiscountsCommand request, CancellationToken cancellationToken)
        {
            var result = await _ownedPropertyService.GetOwnedPropertyAsync(request.PropertyId, request.CurrentUserId, cancellationToken);

            if (!result.IsSuccess)
                return OperationResult<Unit>.Fail(result.Errors.ToArray());

            var property = result.Data!;
            var input = request.DiscountsInput;
            var newDiscounts = new List<Discount>();

            SaveDiscount(property, DiscountTypeEnum.LastMinute, input.LastMinuteDiscountEnabled, input.LastMinuteDiscountPercent,
                d => d.DaysBeforeCheckIn = 14, newDiscounts);
            SaveDiscount(property, DiscountTypeEnum.Monthly, input.MonthlyDiscountEnabled, input.MonthlyDiscountPercent,
                d => d.MinNights = 28, newDiscounts);
            SaveDiscount(property, DiscountTypeEnum.NewListingPromo, input.NewListingDiscountEnabled, input.NewListingDiscountPercent,
                d => d.MaxUses = 3, newDiscounts);
            SaveDiscount(property, DiscountTypeEnum.Weekly, input.WeeklyDiscountEnabled, input.WeeklyDiscountPercent,
                d => d.MinNights = 7, newDiscounts);

            await _discountRepository.AddRangeAsync(newDiscounts, cancellationToken);
            await _discountRepository.SaveChangesAsync(cancellationToken);

            return OperationResult<Unit>.Success(new Unit());
        }

        private static void SaveDiscount(Property property, DiscountTypeEnum type, bool enabled, decimal percent,
            Action<Discount> setRules, List<Discount> newDiscounts)
        {
            var sameType = property.Discounts
                .Where(d => d.Type == type)
                .OrderByDescending(d => d.Id)
                .ToList();

            var discount = sameType.FirstOrDefault();

            foreach (var old in sameType.Skip(1))
                old.IsActive = false;

            if (discount == null)
            {
                if (!enabled)
                    return;

                discount = new Discount
                {
                    Type = type,
                    PropertyId = property.Id
                };
                setRules(discount);
                newDiscounts.Add(discount);
            }

            discount.Percentage = percent;
            discount.IsActive = enabled;
        }
    }
}