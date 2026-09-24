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

            //var existing = await _discountRepository.GetActiveByPropertyIdAsync(property.Id, false, ct:cancellationToken);
            var existing = property.Discounts.Where(d => d.IsActive);

            var desiredTypes = new HashSet<DiscountTypeEnum>();
            var discounts = new List<Discount>();

            if (request.DiscountsInput.LastMinuteDiscountEnabled)
            {
                desiredTypes.Add(DiscountTypeEnum.LastMinute);
                var discount = existing.FirstOrDefault(d => d.Type == DiscountTypeEnum.LastMinute);
                if(discount != null)
                {
                    discount.Percentage = request.DiscountsInput.LastMinuteDiscountPercent;
                }
                else
                {
                    discounts.Add(new Discount
                    {
                        Type = DiscountTypeEnum.LastMinute,
                        Percentage = request.DiscountsInput.LastMinuteDiscountPercent,
                        IsActive = true,
                        PropertyId = property.Id,
                        DaysBeforeCheckIn = 14
                    });
                }
            }
            if (request.DiscountsInput.MonthlyDiscountEnabled)
            {
                desiredTypes.Add(DiscountTypeEnum.Monthly);
                var discount = existing.FirstOrDefault(d => d.Type == DiscountTypeEnum.Monthly);
                if (discount != null)
                {
                    discount.Percentage = request.DiscountsInput.MonthlyDiscountPercent;
                }
                else
                    discounts.Add(new Discount
                    {
                        Type = DiscountTypeEnum.Monthly,
                        Percentage = request.DiscountsInput.MonthlyDiscountPercent,
                        IsActive = true,
                        PropertyId = property.Id,
                        MinNights = 28
                    });
            }
            if (request.DiscountsInput.NewListingDiscountEnabled)
            {
                desiredTypes.Add(DiscountTypeEnum.NewListingPromo);
                var discount = existing.FirstOrDefault(d => d.Type == DiscountTypeEnum.NewListingPromo);
                if (discount != null)
                {
                    discount.Percentage = request.DiscountsInput.NewListingDiscountPercent;
                }
                else
                    discounts.Add(new Discount
                    {
                        Type = DiscountTypeEnum.NewListingPromo,
                        Percentage = request.DiscountsInput.NewListingDiscountPercent,
                        IsActive = true,
                        PropertyId = property.Id,
                        MaxUses = 3
                    });
            }
            if (request.DiscountsInput.WeeklyDiscountEnabled)
            {
                desiredTypes.Add(DiscountTypeEnum.Weekly);
                var discount = existing.FirstOrDefault(d => d.Type == DiscountTypeEnum.Weekly);
                if (discount != null)
                {
                    discount.Percentage = request.DiscountsInput.WeeklyDiscountPercent;
                }
                else
                    discounts.Add(new Discount
                    {
                        Type = DiscountTypeEnum.Weekly,
                        Percentage = request.DiscountsInput.WeeklyDiscountPercent,
                        IsActive = true,
                        PropertyId = property.Id,
                        MinNights = 7
                    });
            }

            await _discountRepository.AddRangeAsync(discounts, cancellationToken);

            var toDelete = (await _discountRepository.GetActiveByPropertyIdAsync(property.Id, noTracking:false, ct:cancellationToken))
                .Where(m => !desiredTypes.Contains(m.Type));

            foreach(var discount in toDelete)
            {
                discount.IsActive = false;
            }

            await _discountRepository.SaveChangesAsync(cancellationToken);

            return OperationResult<Unit>.Success(new Unit());
        }
    }
}
