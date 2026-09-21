using MediatR;
using Renty.Application.Commands.PropertyCommands;
using Renty.Application.Common;
using Renty.Application.Helpers;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.PropertyHandlers
{
    public class SavePropertyBasicsHandler : IRequestHandler<SavePropertyBasicsCommand, OperationResult<Guid>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly OwnedPropertyService _ownedPropertyService;


        public SavePropertyBasicsHandler(IPropertyRepository propertyRepository, OwnedPropertyService ownedPropertyService)
        {
            _propertyRepository = propertyRepository;
            _ownedPropertyService = ownedPropertyService;
        }
        public async Task<OperationResult<Guid>> Handle(SavePropertyBasicsCommand request, CancellationToken cancellationToken)
        {

            var result = await _ownedPropertyService.GetOwnedPropertyAsync(request.PropertyId, request.CurrentUserId, cancellationToken);

            if (!result.IsSuccess)
                return OperationResult<Guid>.Fail(result.Errors.ToArray());

            var property = result.Data!;

            if (request.MaxGuests <= 0 || request.MaxGuests >= 50)
                return OperationResult<Guid>.Fail("The number of guests must be between 1 and 50");

            if(request.BathroomsCount < 0 || request.BedsCount < 0 || request.BedroomsCount < 0)
                return OperationResult<Guid>.Fail("Quantity data cannot be less than zero.");

            if(property.Details == null)
            {
                var details = new PropertyDetails
                {
                    MaxGuests = request.MaxGuests,
                    BedroomsCount = request.BedroomsCount,
                    BathroomsCount = request.BathroomsCount,
                    BedsCount = request.BedsCount,
                    PropertyId = property.Id
                };
                property.Details = details;
            }
            else
            {
                property.Details.MaxGuests = request.MaxGuests;
                property.Details.BedroomsCount = request.BedroomsCount;
                property.Details.BathroomsCount = request.BathroomsCount;
                property.Details.BedsCount = request.BedsCount;
            }

            await _propertyRepository.UpdateAsync(property, cancellationToken);

            return OperationResult<Guid>.Success(request.PropertyId);
        }
    }
}
