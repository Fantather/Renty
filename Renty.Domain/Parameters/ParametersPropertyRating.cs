using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Domain.Parameters
{
    public record ParametersPropertyRating
    {
        public decimal Cleanliness;
        public decimal Accuracy;
        public decimal CheckIn;
        public decimal Communication;
        public decimal Location;
        public decimal Value;
    }
}

