using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetProperties;
using Renty.Application.Queries;
using Renty.Domain.Interfaces;
using Renty.Domain.Parameters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers
{
    public class GetPropertiesHandler : IRequestHandler<GetPropertiesQuery, OperationResult<GetPropertiesResponse>>
    {
        private readonly IPropertyRepository _propertyRepository;

        public GetPropertiesHandler(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }
        public async Task<OperationResult<GetPropertiesResponse>> Handle(GetPropertiesQuery request, CancellationToken cancellationToken)
        {
            var param = new ParametersPropertiesForCatalog
            {
                Skip = (request.Page - 1) * request.PageSize,
                PageSize = request.PageSize,
                SortBy = request.SortBy,
                CityId = request.CityId,
                CategoryId = request.CategoryId,
                CategorySlug = request.CategorySlug,
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                GuestCount = request.GuestCount
            };

            var properties = await _propertyRepository.GetPropertiesForCatalogAsync(param, cancellationToken);

            return OperationResult<GetPropertiesResponse>.Success(new GetPropertiesResponse { });
        }
    }
}
