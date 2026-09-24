using Renty.Domain.Interfaces;
using Renty.Domain.Models.Properties;
using Renty.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Infrastructure.Repository
{
    public class PropertyTagRepository : GenericRepository<PropertyTag>, IPropertyTagRepository
    {
        public PropertyTagRepository(AppDbContext context) : base(context)
        {
        }
    }
}
