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

        // Связь с Property
        public Guid PropertyId { get; set; }
        //[ForeignKey(nameof(PropertyId))]
        public virtual Property Property { get; set; }

    }
}
