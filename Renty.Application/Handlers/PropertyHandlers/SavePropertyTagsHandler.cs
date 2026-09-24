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
        private readonly ITagRepository _tagRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly OwnedPropertyService _ownedPropertyService;
        private readonly IPropertyTagRepository _propertyTagRepository;
        public SavePropertyTagsHandler(
            ITagRepository tagRepository,
            IPropertyRepository propertyRepository,
            OwnedPropertyService ownedPropertyService,
            IPropertyTagRepository propertyTagRepository)
        {
            _tagRepository = tagRepository;
            _propertyRepository = propertyRepository;
            _ownedPropertyService = ownedPropertyService;
            _propertyTagRepository = propertyTagRepository;
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

            var existingIds = await _tagRepository.GetExistingIdsAsync(request.TagIds, cancellationToken);

            var missingIds = existingIds.Except(request.TagIds);

            if (missingIds.Any())
                return OperationResult<Guid>.Fail($"Unknown tags: {string.Join(", ", missingIds)}");

            var currentIds = property.PropertyTags
                .Select(t=>t.TagId)
                .ToHashSet();

            var toAdd = requestIds.Where(id => !currentIds.Contains(id));
            var newTags = new List<PropertyTag>();

            foreach(var id in toAdd)
            {
                var tag = new PropertyTag
                {
                    PropertyId = property.Id,
                    TagId = id
                };

                property.PropertyTags.Add(tag);
                newTags.Add(tag);
            }
            await _propertyTagRepository.AddRangeAsync(newTags,cancellationToken);

            var toRemove = currentIds.Where(id => !requestIds.Contains(id));

            foreach(var tag in property.PropertyTags.Where(t => toRemove.Contains(t.TagId)))
            {
                tag.IsActive = false;
            }

            await _propertyRepository.SaveChangesAsync(cancellationToken);

            return OperationResult<Guid>.Success(property.Id);
        }
    }
}
