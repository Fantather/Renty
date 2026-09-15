using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Domain.ServiceModels.Places
{
    public sealed class AddressSuggestionDto
    {
        public string PlaceId { get; set; }
        public string Text { get; set; }
    }
}
