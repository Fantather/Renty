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
    public class SavePropertyLocationVisibilityHandler : IRequestHandler<SavePropertyLocationVisibilityCommand, OperationResult<Guid>>
    {
        private readonly OwnedPropertyService _ownedPropertyService;
        private readonly IPropertyRepository _propertyRepository;
        public SavePropertyLocationVisibilityHandler(
            OwnedPropertyService ownedPropertyService,
            IPropertyRepository propertyRepository)
        {
            _ownedPropertyService = ownedPropertyService;
            _propertyRepository = propertyRepository;
        }
        public async Task<OperationResult<Guid>> Handle(SavePropertyLocationVisibilityCommand request, CancellationToken cancellationToken)
        {
            var result = await _ownedPropertyService.GetOwnedPropertyAsync(request.PropertyId, request.CurrentUserId, cancellationToken);

            if (!result.IsSuccess)
                return OperationResult<Guid>.Fail(result.Errors.ToArray());

            var property = result.Data!;

            property.ShowExactLocation = request.ShowExactLocation;
            await _propertyRepository.UpdateAsync(property, cancellationToken);

            return OperationResult<Guid>.Success(property.Id);
        }
    }
}
