
using AutoMapper;
using MediatR;
using Microsoft.VisualBasic;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using Renty.Application.Commands.PropertyCommands;
using Renty.Application.Common;
using Renty.Application.DTOs.CreateProperty;
using Renty.Application.Helpers;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.Locations;
using Renty.Domain.Models.LookupsTables;
using Renty.Domain.Models.Properties;
using Renty.Infrastructure.Helpers;
using Renty.Infrastructure.Repository;

namespace Renty.Application.Handlers.PropertyHandlers
{
    public class CreatePropertyCommandHandler : IRequestHandler<SavePropertyAddressCommand, OperationResult<Guid>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly AddressResolverService _addressResolverService;
        private readonly IMapper _mapper;
        private readonly OwnedPropertyService _ownedPropertyService;

        public CreatePropertyCommandHandler(
        IPropertyRepository propertyRepository,
        AddressResolverService addressResolverService,
        IMapper mapper,
        OwnedPropertyService ownedPropertyService)
        {
            _propertyRepository = propertyRepository;
            _addressResolverService = addressResolverService;
            _mapper = mapper;
            _ownedPropertyService = ownedPropertyService;
        }

        public async Task<OperationResult<Guid>> Handle(SavePropertyAddressCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Data;

            var addressResult = await _addressResolverService.ResolveAsync(dto, cancellationToken);
            if(!addressResult.IsSuccess)
                return OperationResult<Guid>.Fail(addressResult.Errors.ToArray());

            // перенос в проперти
            var property = _mapper.Map<Property>(dto);

            if (request.Data.PropertyId.HasValue)
            {
                var result = await _ownedPropertyService.GetOwnedPropertyAsync(request.Data.PropertyId.Value, request.Data.HostId, cancellationToken);

                if (!result.IsSuccess)
                    return OperationResult<Guid>.Fail(result.Errors.ToArray());

                property = result.Data!;

                property.AddressId = addressResult.Data!.Id;
                property.CityId = addressResult.Data!.CityId!;
                property.CountryId = addressResult.Data.City.CountryId;
                property.AddressId = addressResult.Data!.Id;

                await _propertyRepository.SaveChangesAsync(cancellationToken);
            }
            else
            {
                property.AddressId = addressResult.Data!.Id;
                //а это айди городов из бд
                property.CityId = addressResult.Data!.CityId!;
                property.CountryId = addressResult.Data.City.CountryId;
                property.AddressId = addressResult.Data!.Id;

                await _propertyRepository.AddAsync(property, cancellationToken);
            }

            return OperationResult<Guid>.Success(property.Id);
        }
    }
}
