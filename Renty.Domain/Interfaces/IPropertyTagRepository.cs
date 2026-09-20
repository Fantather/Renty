using Renty.Domain.Models.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Domain.Interfaces
{
    public interface IPropertyTagRepository : IGenericRepository<Tag>
    {
        Task<IEnumerable<Guid>> GetExistingIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default);

        Task<IEnumerable<Tag>> GetTagsByPropertyIdAsync(Guid propertyId, bool activeOnly = true, CancellationToken ct = default);
    }
}
