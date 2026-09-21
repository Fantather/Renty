using MediatR;
using Renty.Application.Commands.PropertyCommands;
using Renty.Application.Common;
using Renty.Application.Helpers;
using Renty.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.PropertyHandlers
{
    public class SavePropertyPricingHandler : IRequestHandler<SavePropertyPricingCommand, OperationResult<Guid>>
    {
        private readonly OwnedPropertyService _ownedPropertyService;
        private readonly IPropertyRepository _propertyRepository;
        public SavePropertyPricingHandler(
            OwnedPropertyService ownedPropertyService,
            IPropertyRepository propertyRepository)
        {
            _ownedPropertyService = ownedPropertyService;
            _propertyRepository = propertyRepository;
        }
        public async Task<OperationResult<Guid>> Handle(SavePropertyPricingCommand request, CancellationToken cancellationToken)
        {
            int currencyLength = 2;

            var result = await _ownedPropertyService.GetOwnedPropertyAsync(request.PropertyId, request.CurrentUserId, cancellationToken);

            if (!result.IsSuccess)
                return OperationResult<Guid>.Fail(result.Errors.ToArray());

            var property = result.Data!;

            if (string.IsNullOrWhiteSpace(request.Currency) || request.Currency.Length > currencyLength)
                return OperationResult<Guid>.Fail("Invalid currency");
            if (request.PricePerNight < 0)
                return OperationResult<Guid>.Fail("Invalid price per night");


            property.PricePerNight = request.PricePerNight;
            property.Currency = request.Currency;
            property.WeekendPricePercent = request.WeekendPricePercent;

            await _propertyRepository.UpdateAsync(property, cancellationToken);

            return OperationResult<Guid>.Success(property.Id);
        }
    }
}
