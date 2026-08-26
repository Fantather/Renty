using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Renty.Domain.Models.Properties;

namespace Renty.Domain.Models.Media
{
    /// <summary>
    /// Изображения комнат
    /// </summary>
    public class RoomImage: Image
    {
        // Связь с House
        public int HouseId { get; set; }
        [ForeignKey(nameof(HouseId))]
        public virtual House House { get; set; }

    }
}
