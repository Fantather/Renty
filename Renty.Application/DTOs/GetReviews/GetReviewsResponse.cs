using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetReviews
{
    /// <summary>
    /// Модель для отображения списка отзывов
    /// </summary>
    public class GetReviewsResponse
    
    {
        public List<ReviewDto> Reviews { get; set; } = new();

        //public int TotalCount { get; set; }
        //public int Page { get; set; }
        //public int PageSize { get; set; }
        //public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    }
}
