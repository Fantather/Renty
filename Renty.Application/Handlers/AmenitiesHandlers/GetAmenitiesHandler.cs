using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetAmenities;
using Renty.Application.Queries;
using Renty.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.AmenitiesHandlers
{
    public class GetAmenitiesHandler : IRequestHandler<GetAmenitiesQuery, OperationResult<List<AmenityItem>>>
    {
        private readonly IAmenityRepository _amenityRepository;
        public GetAmenitiesHandler(IAmenityRepository amenityRepository)
        {
            _amenityRepository = amenityRepository;
        }
        public async Task<OperationResult<List<AmenityItem>>> Handle(GetAmenitiesQuery request, CancellationToken cancellationToken)
        {
            return OperationResult<List<AmenityItem>>.Success((await _amenityRepository.GetAllAsync(cancellationToken))
                .Select(a => new AmenityItem
                {
                    Id = a.Id,
                    Description = a.Description,
                    Name = a.Name,
                    IconName = a.IconUrl ?? "star"
                }).ToList());
        }
    }
}
