using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.CreateProperty;
using Renty.Application.Helpers;
using Renty.Application.Queries.Property;
using Renty.Domain.Interfaces;
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

            // Тестовый мэппинг

            var tagIds = property.PropertyTags.Select(t => t.TagId).ToList();

            var amenityIds = property.PropertyAmenities.Select(a => a.AmenityId).ToList();

            var images = property.PropertyImages.Select(i => new OrderedImageDto { ImageId = i.Id, IsPrimary = i.IsPrimary, ImageUrl = i.ImageUrl, DisplayOrder = i.DisplayOrder }).ToList();


            var propertyDraft = new PropertyDraftDto
            {
                Id = property.Id,
                Name = property.Name,
                CategoryId = property.CategoryId,
                Description = property.Description,
                CountryId = property.CountryId,
                CityId = property.CityId,
                PlaceId = property.Address.PlaceId,
                Address = property.Address.FullAddress,
                Street = property.Address.Street,
                Latitude = property.Address.Location?.Coordinate.Y,
                Longitude = property.Address.Location?.Coordinate.X,
                Floor = property?.Details.Floor,
                FloorsCount = property?.Details.FloorsCount,
                BathroomsCount = property?.Details.BathroomsCount,
                BedroomsCount = property?.Details.BedroomsCount,
                BedsCount = property?.Details.BedsCount,
                MaxGuests = property?.Details.MaxGuests,
                Highlights = new(),
                HouseRules = property?.HouseRules,
                CheckInTime = property?.CheckInTime,
                CheckOutTime = property?.CheckOutTime,
                PricePerNight = property?.PricePerNight,
                WeekendPricePercent = 0,
                Currency = property?.Currency,
                TagIds = tagIds,
                AmenityIds = amenityIds,
                Images = images,
                Discounts = null,
                District = property?.Address.District,
                InstantBookEnabled = null,
                ShowExactLocation = null
            };

            return OperationResult<PropertyDraftDto>.Success(propertyDraft);
        }
    }
}
