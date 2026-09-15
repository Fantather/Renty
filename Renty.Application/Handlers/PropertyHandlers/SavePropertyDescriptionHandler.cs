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
    public class SavePropertyDescriptionHandler : IRequestHandler<SavePropertyDescriptionCommand, OperationResult<Guid>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly OwnedPropertyService _ownedPropertyService;
        public SavePropertyDescriptionHandler(IPropertyRepository propertyRepository, OwnedPropertyService ownedPropertyService)
        {
            _propertyRepository = propertyRepository;
            _ownedPropertyService = ownedPropertyService;
        }
        public async Task<OperationResult<Guid>> Handle(SavePropertyDescriptionCommand request, CancellationToken cancellationToken)
        {
            var result = await _ownedPropertyService.GetOwnedPropertyAsync(request.PropertyId, request.CurrentUserId, cancellationToken);

            if (!result.IsSuccess)
                return OperationResult<Guid>.Fail(result.Errors.ToArray());

            var property = result.Data!;

            if (string.IsNullOrWhiteSpace(request.Description))
                return OperationResult<Guid>.Fail("Description is null or white space");

            property.Description = request.Description;

            await _propertyRepository.UpdateAsync(property);

            return OperationResult<Guid>.Success(request.PropertyId);
        }
    }
}
