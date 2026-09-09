
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Renty.Domain.Models.LookupsTables
{
    public abstract class Lookup
    /// <summary>
    /// Базовый абстрактный класс для всех справочников
    /// </summary>
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}
