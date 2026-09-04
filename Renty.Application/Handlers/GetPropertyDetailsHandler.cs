using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.Common;
using Renty.Application.DTOs.GetProperty;
using Renty.Application.Queries;
using Renty.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers
{
    public class GetPropertyDetailsHandler : IRequestHandler<GetPropertyDetailsQuery, OperationResult<GetPropertyDetailsResponse>>
    {
        private readonly IPropertyRepository _propertyRepository;
        public GetPropertyDetailsHandler(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;   
        }
        public async Task<OperationResult<GetPropertyDetailsResponse>> Handle(GetPropertyDetailsQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.PropertySlug)) return OperationResult<GetPropertyDetailsResponse>.Fail("PropertySlug is null or empty");

            var property = await _propertyRepository.GetPropertyWithDetailsAsync(request.PropertySlug, cancellationToken);

            if (property == null)
                return OperationResult<GetPropertyDetailsResponse>.Fail("Property not found");


            // Черновой вариант маппинга

            var category = new CategoryDto
            {
                Id = property.CategoryId,
                Slug = property.Slug,
                Name = property.Name,
                Description = property.Category.Description,
                ImageUrl = property.Category.ImageUrl
            };

            var tags = property.PropertyTags.Select(t => 
                new TagDto {
                    Name = t.Tag.Name,
                    IconUrl = t.Tag.IconUrl,  // IconUrl должна быть Guid или это ошибка?
                    IconId = t.Tag.IconId
                }
            ).ToList(); 

            var amenities = property.PropertyAmenities.Select(a => new AmenitiesDto
                {
                    Name = a.Amenity.Name,
                    Description = a.Amenity.Description,
                    IconUrl = a.Amenity.IconUrl
                }
            ).ToList();

            var rooms = property.Rooms.Select(r => new RoomDto
            {
                Name = r.Name,
                Area =r.Area,
                Amenities = r.RoomAmenities.Select(a => new AmenitiesDto { Name = a.Amenity.Name, Description = a.Amenity.Description, IconUrl = a.Amenity.IconUrl}).ToList(),
                BedsCount = r.BedsCount,
                Description = r.Description,
                IsSharedSpace = r.IsSharedSpace,
                Images = r.Images.Select(i => new ImageDto { IsPrimary = i.IsPrimary, CreatedAt = i.CreatedAt, DisplayOrder = i.DisplayOrder, ImageUrl = i.ImageUrl}).ToList(),
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                RoomType = new RoomTypeDto { Name = r.RoomType.Name, Description = r.RoomType.Description }
            }).ToList();

            var host = new HostDto
            {
                FullName = $"{property.Host.FirstName} {property.Host.LastName}",
                Info = "property.Host.Info",
                AvatarUrl = property.Host.AvatarUrl,
                IsVerified = true, // property.Host.IsVerified
                Languages = "", // property.Host.Languages
                CreatedAt = property.Host.CreatedAt,
                ResponseSpeed = "100%",
            };

            var propertyDto = new GetPropertyDetailsResponse
            {
                Slug = property.Slug,
                PricePerNight = property.PricePerNight,
                Tags = tags,
                Category = category,
                PropertyName = property.Name,
                CityName = property.City.Name,
                CountryName = property.Country.Name,
                Address = property.Address,
                Street = property.Street,
                ReviewsCount = property.ReviewsCount,
                AverageRating = property.AverageRating,
                Images = property.PropertyImages.Select(i => new ImageDto { IsPrimary = i.IsPrimary, ImageUrl = i.ImageUrl, DisplayOrder = 0, CreatedAt = i.CreatedAt }).ToList(),
                Floor = property.Details.Floor,
                FloorsCount = property.Details.FloorsCount,
                BedsCount = property.Details.BedsCount,
                BathroomsCount = property.Details.BathroomsCount,
                BedroomsCount = property.Details.BathroomsCount,
                MaxGuests = property.Details.MaxGuests,
                HouseRules = property.HouseRules,
                RoomsCount = property.Rooms.Count,
                Description = property.Description,
                Amenities = amenities,
                Rooms = rooms,
                Currency = property.Currency,
                District = property.District,
                Host = host,
                CreatedAt = property.CreatedAt,
                UpdatedAt = property.UpdatedAt,
                IsFavorite = request.UserId.HasValue ? property.Favorites.Any(f => f.UserId == request.UserId) : false
            };

            return OperationResult<GetPropertyDetailsResponse>.Success(propertyDto);
        }
    }
}
