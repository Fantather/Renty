using MediatR;
using Renty.Application.Commands.PropertyCommands;
using Renty.Application.Common;
using Renty.Application.Helpers;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.LookupsTables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.PropertyHandlers
{
    public class PublishPropertyHandler : IRequestHandler<PublishPropertyCommand, OperationResult<Unit>>
    {
        private readonly OwnedPropertyService _ownedPropertyService;
        private readonly IPropertyRepository _propertyRepository;
        public PublishPropertyHandler(
             OwnedPropertyService ownedPropertyService,
             IPropertyRepository propertyRepository)
        {
            _ownedPropertyService = ownedPropertyService;
            _propertyRepository = propertyRepository;
        }
        public async Task<OperationResult<Unit>> Handle(PublishPropertyCommand request, CancellationToken cancellationToken)
        {
            var result = await _ownedPropertyService.GetOwnedPropertyAsync(request.PropertyId, request.CurrentUserId, cancellationToken);

            if (!result.IsSuccess)
                return OperationResult<Unit>.Fail(result.Errors.ToArray());

            var property = result.Data!;

            var missingFields = PropertyPublishValidator.Validate(property);

            if (missingFields.Any())
                return OperationResult<Unit>.Fail($"Required fields are not filled in: {string.Join(", ", missingFields)}");

            property.Status = PropertyStatusEnum.Active;
            await _propertyRepository.UpdateAsync(property, cancellationToken);

            return OperationResult<Unit>.Success(Unit.Value);
        }
    }
}
