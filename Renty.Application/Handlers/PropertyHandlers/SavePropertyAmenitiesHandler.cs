using MediatR;
using Renty.Application.Commands.PropertyCommands;
using Renty.Application.Common;
using Renty.Application.Helpers;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.Properties.Anemities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.PropertyHandlers
{
    public class SavePropertyAmenitiesHandler : IRequestHandler<SavePropertyAmenitiesCommand, OperationResult<Guid>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IAmenityRepository _amenityRepository;
        private readonly OwnedPropertyService _ownedPropertyService;
        public SavePropertyAmenitiesHandler(IPropertyRepository propertyRepository, IAmenityRepository amenityRepository, OwnedPropertyService ownedPropertyService)
        {
            _propertyRepository = propertyRepository;
            _ownedPropertyService = ownedPropertyService;
            _amenityRepository = amenityRepository;
        }
        public async Task<OperationResult<Guid>> Handle(SavePropertyAmenitiesCommand request, CancellationToken cancellationToken)
        {

            var result = await _ownedPropertyService.GetOwnedPropertyAsync(request.PropertyId, request.CurrentUserId, cancellationToken);

            if (!result.IsSuccess)
                return OperationResult<Guid>.Fail(result.Errors.ToArray());

            // Убираем дубликаты
            var requestedIds = request.Amenities.Distinct().ToList();

            if (!requestedIds.Any())
                return OperationResult<Guid>.Fail("The list of amenities is empty");

            // Проверка существования айди до попытки сохранить
            var existingIds = await _amenityRepository.GetExistingIdsAsync(requestedIds, cancellationToken);

            var missingIds = requestedIds.Except(existingIds).ToList();

            if (missingIds.Any())
                return OperationResult<Guid>.Fail($"Unknown amenities: {string.Join(", ", missingIds)}");
            

            var property = result.Data!;
            var currentIds = (await _amenityRepository.GetAmenitiesByPropertyIdAsync(property.Id, ct: cancellationToken))
                .Select(a => a.Id)
                .ToHashSet();

            var toAdd = requestedIds.Where(id => !currentIds.Contains(id));


            foreach(var amenityId in toAdd)
            {
                property.PropertyAmenities.Add(new PropertyAmenity
                {
                    PropertyId = property.Id,
                    AmenityId = amenityId,
                    IsActive = true
                });
            }

            var toRemove = currentIds.Where(id => !requestedIds.Contains(id)).ToList();

            foreach (var amenity in property.PropertyAmenities.Where(pa => toRemove.Contains(pa.Id)))
                amenity.IsActive = false;

            await _propertyRepository.UpdateAsync(property, cancellationToken);

            return OperationResult<Guid>.Success(request.PropertyId);
        }
    }
}
