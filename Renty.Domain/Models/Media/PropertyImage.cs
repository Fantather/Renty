using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Renty.Domain.Models.Properties;
using Renty.Domain.Models.LookupsTables;

namespace Renty.Domain.Models.Media
{
    /// <summary>
    /// Изображения объектов недвижимости
    /// </summary>
    public class PropertyImage:Image
    {

        //приналдежит квартире ТОЧНО
        public Guid PropertyId { get; set; }
        [ForeignKey(nameof(PropertyId))]
        public virtual Property Property { get; set; } = null!;

        ///если null - то это фасад
        public Guid? RoomId { get; set; }
        [ForeignKey(nameof(RoomId))]
        public virtual Room? Room { get; set; }
    }

}

