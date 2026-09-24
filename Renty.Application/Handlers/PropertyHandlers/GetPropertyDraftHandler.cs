using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.CreateProperty;
using Renty.Application.Helpers;
using Renty.Application.Queries.Property;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.LookupsTables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.PropertyHandlers
{
    public class GetPropertyDraftHandler : IRequestHandler<GetPropertyDraftQuery, OperationResult<PropertyDraftDto>>
    {
        private readonly OwnedPropertyService _ownedPropertyService;
        public GetPropertyDraftHandler(OwnedPropertyService ownedPropertyService)
        {
            _ownedPropertyService = ownedPropertyService;
        }
        public async Task<OperationResult<PropertyDraftDto>> Handle(GetPropertyDraftQuery request, CancellationToken cancellationToken)
        {
            if (!request.PropertyId.HasValue)
                return OperationResult<PropertyDraftDto>.Fail("PropertyId is null");

            var result = await _ownedPropertyService.GetOwnedPropertyAsync(request.PropertyId.Value, request.CurrentUserId, cancellationToken);

            if (!result.IsSuccess)
                return OperationResult<PropertyDraftDto>.Fail(result.Errors.ToArray());

            var property = result.Data!;


            var tagIds = property.PropertyTags.Select(t => t.TagId).ToList();

            var amenityIds = property.PropertyAmenities.Select(a => a.AmenityId).ToList();

            var images = property.PropertyImages.Select(i => new OrderedImageDto { ImageId = i.Id, IsPrimary = i.IsPrimary, ImageUrl = i.ImageUrl, DisplayOrder = i.DisplayOrder }).ToList();

            var discounts = new DiscountsInputDto();

            foreach(var discount in property.Discounts.Where(d => d.IsActive))
            {
                switch (discount.Type)
                {
                    case DiscountTypeEnum.LastMinute:
                        discounts.LastMinuteDiscountEnabled = true;
                        discounts.LastMinuteDiscountPercent = Convert.ToInt32(discount.Percentage);
                        break;
                    case DiscountTypeEnum.NewListingPromo:
                        discounts.NewListingDiscountEnabled = true;
                        discounts.NewListingDiscountPercent = Convert.ToInt32(discount.Percentage);
                        break;
                    case DiscountTypeEnum.Monthly:
                        discounts.MonthlyDiscountEnabled = true;
                        discounts.MonthlyDiscountPercent = Convert.ToInt32(discount.Percentage);
                        break;
                    case DiscountTypeEnum.Weekly:
                        discounts.WeeklyDiscountEnabled = true;
                        discounts.WeeklyDiscountPercent = Convert.ToInt32(discount.Percentage);
                        break;
                }
                
            }

            var propertyDraft = new PropertyDraftDto
            {
                Id = property.Id,
                Name = property?.Name,
                CategoryId = property?.CategoryId,
                Description = property?.Description,
                CountryName = string.IsNullOrEmpty(property.Country?.Name) ? property.Country?.NameRu : property.Country?.Name,
                CityName = string.IsNullOrEmpty(property.City?.Name) ? property.City?.NameRu : property.City?.Name,
                PlaceId = property.Address?.PlaceId,
                Address = property.Address?.FullAddress,
                Street = property.Address?.Street,
                Latitude = property.Address?.Latitude,
                Longitude = property.Address?.Longitude,
                Floor = property?.Details?.Floor,
                FloorsCount = property?.Details?.FloorsCount,
                BathroomsCount = property?.Details?.BathroomsCount,
                BedroomsCount = property?.Details?.BedroomsCount,
                BedsCount = property?.Details?.BedsCount,
                MaxGuests = property?.Details?.MaxGuests,
                HouseRules = property?.HouseRules,
                CheckInTime = property?.CheckInTime,
                CheckOutTime = property?.CheckOutTime,
                PricePerNight = property?.PricePerNight,
                WeekendPricePercent = property.WeekendPricePercent.HasValue ? property.WeekendPricePercent.Value : 0,
                Currency = property?.Currency,
                TagIds = tagIds,
                AmenityIds = amenityIds,
                Images = images,
                Discounts = discounts,
                District = property?.Address?.District,
                InstantBook = property?.InstantBook,
                ShowExactLocation = property?.ShowExactLocation,
            };

            return OperationResult<PropertyDraftDto>.Success(propertyDraft);
        }
    }
}
