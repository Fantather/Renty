
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

        public CreatePropertyCommandHandler(
        IPropertyRepository propertyRepository,
        AddressResolverService addressResolverService,
        IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _addressResolverService = addressResolverService;
            _mapper = mapper;
        }

        public async Task<OperationResult<Guid>> Handle(SavePropertyAddressCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Data;

            var addressResult = await _addressResolverService.ResolveAsync(dto, cancellationToken);
            if(!addressResult.IsSuccess)
                return OperationResult<Guid>.Fail(addressResult.Errors.ToArray());

            // перенос в проперти
            var property = _mapper.Map<Property>(dto);
            property.AddressId = addressResult.Data.Id;
            //а это айди городов из бд
            property.CityId = addressResult.Data!.CityId!;
            property.CountryId = addressResult.Data.City.CountryId;
            property.AddressId = addressResult.Data!.Id;

            await _propertyRepository.AddAsync(property, cancellationToken);

            return OperationResult<Guid>.Success(property.Id);
        }
    }
}
