using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.GetProperty
{
    public class RoomTypeDto
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        //// Порядок отображения
        //public int DisplayOrder { get; set; } = 0;

    }
}
