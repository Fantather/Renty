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
    public class SavePropertyTagsHandler : IRequestHandler<SavePropertyTagsCommand, OperationResult<Guid>>
    {
        private readonly IPropertyTagRepository _propertyTagRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly OwnedPropertyService _ownedPropertyService;
        public SavePropertyTagsHandler(
            IPropertyTagRepository propertyTagRepository,
            IPropertyRepository propertyRepository,
            OwnedPropertyService ownedPropertyService)
        {
            _propertyTagRepository = propertyTagRepository;
            _propertyRepository = propertyRepository;
            _ownedPropertyService = ownedPropertyService;
        }
        public async Task<OperationResult<Guid>> Handle(SavePropertyTagsCommand request, CancellationToken cancellationToken)
        {
            var result = await _ownedPropertyService.GetOwnedPropertyAsync(request.PropertyId, request.CurrentUserId, cancellationToken);

            if (!result.IsSuccess)
                return OperationResult<Guid>.Fail(result.Errors.ToArray());

            var property = result.Data!;

            if (!request.TagIds.Any())
                return OperationResult<Guid>.Fail("The list of tags is empty");

            var requestIds = request.TagIds.Distinct().ToList();

            var existingIds = await _propertyTagRepository.GetExistingIdsAsync(request.TagIds, cancellationToken);

            var missingIds = existingIds.Except(request.TagIds);

            if (missingIds.Any())
                return OperationResult<Guid>.Fail($"Unknown tags: {string.Join(", ", missingIds)}");

            var currentIds = (await _propertyTagRepository.GetTagsByPropertyIdAsync(property.Id, ct: cancellationToken))
                .Select(t=>t.Id)
                .ToHashSet();

            var toAdd = requestIds.Where(id => !currentIds.Contains(id));

            foreach(var id in toAdd)
            {
                property.PropertyTags.Add(new PropertyTag
                {
                    PropertyId = property.Id,
                    TagId = id
                });
            }

            var toRemove = currentIds.Where(id => !requestIds.Contains(id));

            foreach(var tag in property.PropertyTags.Where(t => toRemove.Contains(t.Id)))
            {
                tag.IsActive = false;
            }

            await _propertyRepository.UpdateAsync(property, cancellationToken);

            return OperationResult<Guid>.Success(property.Id);
        }
    }
}
