using Renty.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.CreateProperty
{
    /// <summary>
    /// Модель для указания последовательности отображения изображения
    /// </summary>
    public class OrderedImageRef
    {
        public OrderedImageType Type { get; set; } // "existing" | "new"
        public Guid? Id { get; set; }              // для existing
        public int? FileIndex { get; set; }        // для new
    }
}
