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
    public class SavePropertyCategoryHandler : IRequestHandler<SavePropertyCategoryCommand, OperationResult<Guid>>
    {
        private readonly IPropertiesCategoryRepository _categoryRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly OwnedPropertyService _ownedPropertyService;
        public SavePropertyCategoryHandler(IPropertiesCategoryRepository categoryRepository, IPropertyRepository propertyRepository, OwnedPropertyService ownedPropertyService)
        {
            _categoryRepository = categoryRepository;
            _propertyRepository = propertyRepository;
            _ownedPropertyService = ownedPropertyService;
        }
        public async Task<OperationResult<Guid>> Handle(SavePropertyCategoryCommand request, CancellationToken cancellationToken)
        {
            var result = await _ownedPropertyService.GetOwnedPropertyAsync(request.PropertyId, request.CurrentUserId, cancellationToken);

            if (!result.IsSuccess)
                return OperationResult<Guid>.Fail(result.Errors.ToArray());

            var property = result.Data!;

            var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);

            if (category == null)
                return OperationResult<Guid>.Fail("The category does not exist");

            property.CategoryId = request.CategoryId;

            await _propertyRepository.UpdateAsync(property, cancellationToken);

            return OperationResult<Guid>.Success(request.PropertyId);
        }
    }
}
