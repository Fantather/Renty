using Renty.Domain.Models.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Helpers
{
    public static class PropertyPublishValidator
    {
        public static List<string> Validate(Property property)
        {
            var missing = new List<string>();

            if (string.IsNullOrWhiteSpace(property.Name))
                missing.Add(nameof(property.Name));

            if (string.IsNullOrWhiteSpace(property.Description))
                missing.Add(nameof(property.Description));

            if (property?.AddressId == null || property.Address == null)
                missing.Add(nameof(property.Address));

            if (property?.PricePerNight <= 0)
                missing.Add(nameof(property.PricePerNight));

            if (property?.PropertyImages == null || !property.PropertyImages.Any())
                missing.Add(nameof(property.PropertyImages));

            if (property?.CategoryId == null || property.Category == null)
                missing.Add(nameof(property.Category));

            if (property?.Discounts == null || !property.Discounts.Any())
                missing.Add(nameof(property.Discounts));

            if (property?.Details == null)
                missing.Add(nameof(property.Details));

            if (property?.PropertyTags == null || !property.PropertyTags.Any())
                missing.Add(nameof(property.PropertyTags));

            if (property?.PropertyAmenities == null || !property.PropertyAmenities.Any())
                missing.Add(nameof(property.PropertyAmenities));

            return missing;
        }
    }
}
