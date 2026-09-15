using Org.BouncyCastle.Asn1.Ocsp;
using Renty.Application.Common;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Helpers
{
    public class OwnedPropertyService
    {
        private readonly IPropertyRepository _propertyRepository;

        public OwnedPropertyService(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }
        public async Task<OperationResult<Property>> GetOwnedPropertyAsync(Guid propertyId, Guid currentUserId, CancellationToken ct = default)
        {
            var property = await _propertyRepository.GetByIdAsync(propertyId, ct);

            if (property == null)
                return OperationResult<Property>.Fail("The property does not exist");

            if (property.HostId != currentUserId)
                return OperationResult<Property>.Fail("Access denied");

            return OperationResult<Property>.Success(property);
        }
    }
}
