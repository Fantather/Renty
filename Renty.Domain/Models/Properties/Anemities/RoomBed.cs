using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Renty.Domain.Models.Properties.Anemities
{
    /// <summary>
    /// Кровати в комнате
    /// </summary>
    public class RoomBed
    {
        public Guid Id { get; set; }

        public Guid RoomId { get; set; }
        [ForeignKey(nameof(RoomId))]
        public virtual House Room { get; set; }

        // Тип кровати (King, Queen, Double, Single, Sofa Bed, etc.)
        public string BedType { get; set; } = string.Empty;

        // Количество кроватей данного типа
        public int Count { get; set; } = 1;


    }
}
