using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Renty.Domain.Models.Locations
{
    /// <summary>
    /// Модель адреса невдижимости
    /// </summary>
    public class Address
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();

        // Идентификатор местоположения
        public string? PlaceId { get; set; }

        // Полный адрес
        public string FullAddress { get; set; } = string.Empty;

        // Улица
        public string? Street { get; set; }

        // Область
        public string? District { get; set; }

        // Координаты недвижимости для отображения на карте
        [Column(TypeName = "geometry (Point, 4326)")]
        public Point? Location { get; set; }

        [NotMapped]
        public double? Latitude => Location?.Y;

        [NotMapped]
        public double? Longitude => Location?.X;

        // Связь с городом
        public Guid CityId { get; set; }

        public virtual City City { get; set; } = null!;

    }
}
