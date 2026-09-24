using Renty.Domain.Interfaces;
using Renty.Domain.Models.Properties.Anemities;
using Renty.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Infrastructure.Repository
{
    public class PropertyAmenityRepository : GenericRepository<PropertyAmenity>, IPropertyAmenityRepository
    {
        public PropertyAmenityRepository(AppDbContext context) : base(context)
        {
        }

    }
}
