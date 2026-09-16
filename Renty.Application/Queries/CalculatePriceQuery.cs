using MediatR;
using Renty.Application.Common;
using Renty.Application.DTOs.GetCategories;
using Renty.Application.DTOs.Pricing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Queries
{
    public class CalculatePriceQuery : IRequest<OperationResult<PriceCalculationResponse>>
    {
        public string Slug { get; set; } = string.Empty;
        public DateOnly CheckIn { get; set; }
        public DateOnly CheckOut { get; set; }
    }
}
