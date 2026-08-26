using System.ComponentModel.DataAnnotations.Schema;

namespace Renty.Domain.Models.Locations
{
    /// <summary>
    /// Города
    /// </summary>
    public class City
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        // Связь со страной
        public int CountryId { get; set; }


        //[ForeignKey(nameof(CountryId))]
        public virtual Country Country { get; set; }

        // Координаты города
        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        // вместо удаления
        public bool IsActive { get; set; } = true;


    }
}
