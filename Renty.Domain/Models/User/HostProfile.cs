using Renty.Domain.Models.Properties;


namespace Renty.Domain.Models.User
{
    public class HostProfile
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();

        // Связь с базовым пользователем
        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; } = null!;

        // Сугубо хостовские фичи
        public string? ResponseSpeed { get; set; }
        public bool IsVerified { get; set; }
        public bool IsSuperhost { get; set; }

        public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}