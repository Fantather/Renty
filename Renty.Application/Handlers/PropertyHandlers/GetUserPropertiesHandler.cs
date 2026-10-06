using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetProperties.UserProperties;
using Renty.Application.Queries.Property;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.LookupsTables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.PropertyHandlers
{
    public class GetUserPropertiesHandler : IRequestHandler<GetUserPropertiesQuery, OperationResult<GetUserPropertiesResponse>>
    {
        private readonly IPropertyRepository _propertyRepository;
        public GetUserPropertiesHandler(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }
        public async Task<OperationResult<GetUserPropertiesResponse>> Handle(GetUserPropertiesQuery request, CancellationToken cancellationToken)
        {
            var properties = await _propertyRepository.GetPropertiesByHostAsync(request.CurrentUserId, ct: cancellationToken);

            if (properties == null || !properties.Any())
                return OperationResult<GetUserPropertiesResponse>.Success(new GetUserPropertiesResponse());

            var response = new GetUserPropertiesResponse
            {
                Drafts = properties.Where(p => p.Status == PropertyStatusEnum.Draft).Select(p => new PropertyCardModel
                {
                    Id = p.Id,
                    City = string.IsNullOrEmpty(p.City.NameRu) ? p.City.Name : p.City.NameRu,
                    Country = string.IsNullOrEmpty(p.Country.NameRu) ? p.Country.Name : p.Country.NameRu,
                    CreatedAt = p.CreatedAt
                }).ToList(),
                Published = properties.Where(p => p.Status == PropertyStatusEnum.Draft).Select(p => new PropertyCardModel
                {
                    Id = p.Id,
                    CategoryName = p.Category!.Name,
                    City = string.IsNullOrEmpty(p.City.NameRu) ? p.City.Name : p.City.NameRu,
                    Country = string.IsNullOrEmpty(p.Country.NameRu) ? p.Country.Name : p.Country.NameRu,
                    Rating = p.AverageRating,
                    PricePerNight = p.PricePerNight,
                    CreatedAt = p.CreatedAt
                }).ToList()
            };

            return OperationResult<GetUserPropertiesResponse>.Success(response);
        }
    }
}
