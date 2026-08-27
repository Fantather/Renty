using Renty.Domain.Models.Media;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Renty.Domain.Models.Properties
{
    /// <summary>
    /// Изображение комнаты
    /// </summary>
    public class RoomImage : Image
    {
        // Связь с комнатой
        public Guid RoomId { get; set; }
        [ForeignKey(nameof(RoomId))]
        public virtual Room Room { get; set; }
    }
}
