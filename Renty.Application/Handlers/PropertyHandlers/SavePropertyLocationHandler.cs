using MediatR;
using Renty.Application.Commands.PropertyCommands;
using Renty.Application.Common;
using Renty.Application.DTOs.CreateProperty;
using Renty.Application.Helpers;
using Renty.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.PropertyHandlers
{
    public class SavePropertyLocationHandler : IRequestHandler<SavePropertyLocationCommand, OperationResult<Guid>>
    {
        private readonly OwnedPropertyService _ownedPropertyService;
        private readonly AddressResolverService _addressResolverService;
        private readonly IPropertyRepository _propertyRepository;
        public SavePropertyLocationHandler(
            OwnedPropertyService ownedPropertyService,
            IPropertyRepository propertyRepository)
        {
            _ownedPropertyService = ownedPropertyService;
            _propertyRepository = propertyRepository;
        }
        public async Task<OperationResult<Guid>> Handle(SavePropertyLocationCommand request, CancellationToken cancellationToken)
        {
            var result = await _ownedPropertyService.GetOwnedPropertyAsync(request.PropertyId, request.CurrentUserId, cancellationToken);

            if (!result.IsSuccess)
                return OperationResult<Guid>.Fail(result.Errors.ToArray());

            var property = result.Data!;

            var resultAddress = await _addressResolverService.ResolveAsync(new SavePropertyAddressDto { Latitude = request.Latitude, Longitude = request.Longitude }, cancellationToken);

            if (!resultAddress.IsSuccess)
                return OperationResult<Guid>.Fail(resultAddress.Errors.ToArray());

            property.AddressId = resultAddress.Data!.Id;

            await _propertyRepository.UpdateAsync(property, cancellationToken);

            return OperationResult<Guid>.Success(property.Id);
        }
    }
}
