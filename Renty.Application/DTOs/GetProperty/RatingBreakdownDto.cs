using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetProperty
{
    public class RatingBreakdownDto
    {
        public decimal Cleanliness { get; set; } // чистота
        public decimal Accuracy { get; set; } // соответствие описанию/фото
        public decimal CheckIn { get; set; } // удобство заезда
        public decimal Communication { get; set; } // общение с хозяином
        public decimal Location { get; set; } // расположение
        public decimal Value { get; set; } // соотношение цена/качество
    }
}
