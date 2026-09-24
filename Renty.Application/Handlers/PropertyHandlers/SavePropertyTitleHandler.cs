using MediatR;
using Renty.Application.Commands.PropertyCommands;
using Renty.Application.Common;
using Renty.Application.Helpers;
using Renty.Domain.Interfaces;
using Renty.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.PropertyHandlers
{
    public class SavePropertyTitleHandler : IRequestHandler<SavePropertyTitleCommand, OperationResult<Guid>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly OwnedPropertyService _ownedPropertyService;
        public SavePropertyTitleHandler(
            IPropertyRepository propertyRepository,
            OwnedPropertyService ownedPropertyService)
        {
            _propertyRepository = propertyRepository;
            _ownedPropertyService = ownedPropertyService;
        }
        public async Task<OperationResult<Guid>> Handle(SavePropertyTitleCommand request, CancellationToken cancellationToken)
        {
            var result = await _ownedPropertyService.GetOwnedPropertyAsync(request.PropertyId, request.CurrentUserId, cancellationToken);

            if (!result.IsSuccess)
                return OperationResult<Guid>.Fail(result.Errors.ToArray());

            var property = result.Data;

            if (string.IsNullOrWhiteSpace(request.Title))
                return OperationResult<Guid>.Fail("Title is null or white space");

            property.Name = request.Title;
            property.Slug = SlugGenerator.GenerateSlug(property.Name, property.Id);

            await _propertyRepository.UpdateAsync(property);

            return OperationResult<Guid>.Success(property.Id);
        }
    }
}
