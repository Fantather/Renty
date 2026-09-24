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

            var input = request.input;

            if (input.MaxGuests <= 0 || input.MaxGuests >= 50)
                return OperationResult<Guid>.Fail("The number of guests must be between 1 and 50");

            if (input.FloorsCount <= 0)
                return OperationResult<Guid>.Fail("The number of floors must be greater than zero");

            if(input.BathroomsCount < 0 || input.BedsCount < 0 || input.BedroomsCount < 0)
                return OperationResult<Guid>.Fail("Quantity data cannot be less than zero.");

            if(property.Details == null)
            {
                var details = new PropertyDetails
                {
                    MaxGuests = input.MaxGuests,
                    BedroomsCount = input.BedroomsCount,
                    BathroomsCount = input.BathroomsCount,
                    BedsCount = input.BedsCount,
                    PropertyId = property.Id,
                    Floor = input.Floor,
                    FloorsCount = input.FloorsCount
                };
                property.Details = details;
            }
            else
            {
                property.Details.MaxGuests = input.MaxGuests;
                property.Details.BedroomsCount = input.BedroomsCount;
                property.Details.BathroomsCount = input.BathroomsCount;
                property.Details.BedsCount = input.BedsCount;
                property.Details.Floor = input.Floor;
                property.Details.FloorsCount = input.FloorsCount;
            }

            await _propertyRepository.UpdateAsync(property, cancellationToken);

            return OperationResult<Guid>.Success(request.PropertyId);
        }
    }
}
