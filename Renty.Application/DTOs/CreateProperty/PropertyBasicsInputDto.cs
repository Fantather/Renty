using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.DTOs.CreateProperty
{
    public class PropertyBasicsInputDto
    {
        public int MaxGuests { get; set; }
        public int BedroomsCount { get; set; }
        public int BedsCount { get; set; }
        public int BathroomsCount { get; set; }
        public int? Floor { get; set; }
        public int FloorsCount { get; set; }
    }
}
