using Renty.Application.DTOs.Common;
using Renty.Application.DTOs.GetCities;
using Renty.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Renty.Application.DTOs.CreateProperty
{
    public class PropertyDraftDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        public string? Description { get; set; }

        public Guid? CategoryId { get; set; }


        public Guid? CountryId { get; set; }

        public Guid? CityId { get; set; }

        public string? PlaceId { get; set; }

        public string? Address { get; set; }

        public string? Street { get; set; }

        public string? District { get; set; }


        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // показывать гостям точную метку или примерный район, пока гость не забронирует
        public bool? ShowExactLocation { get; set; }

        // денежная информация
        public decimal? PricePerNight { get; set; }

        public string? Currency { get; set; }

        // наценка на пятницу/субботу в процентах ("Коэффициент выходных")
        public int? WeekendPricePercent { get; set; }

        public TimeSpan? CheckInTime { get; set; }
        public TimeSpan? CheckOutTime { get; set; }

        public string? HouseRules { get; set; }

        // false = сначала подтверждаем бронирования вручную, true = мгновенное бронирование
        public bool? InstantBookEnabled { get; set; }

        // Заполняем что есть в нашей квартирке. Так как я не привязівала комнаті к прям созданию типа комнаті, то оно так
        public int? MaxGuests { get; set; }
        public int? BedsCount { get; set; }
        public int? BedroomsCount { get; set; }
        public int? BathroomsCount { get; set; }
        public int? Floor { get; set; }
        public int? FloorsCount { get; set; }

        // удобства
        public List<Guid> AmenityIds { get; set; } = new();

        //теги квартиры .\Renty.Domain\Models\Properties\PropertyTag.cs
        public List<Guid> TagIds { get; set; } = new();

        // отличительные черты для описания (не PropertyTag) — фиксированный набор, максимум 2
        public List<string> Highlights { get; set; } = new();

        // изображения с указанием последовательности
        public List<OrderedImageDto> Images { get; set; } = new();

        // можно задать скидки
        public DiscountsInputDto? Discounts { get; set; }
    }
}
