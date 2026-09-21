using Renty.Domain.Models.LookupsTables;

namespace Renty.Application.DTOs.GetUser
{
    public class EditUserProfileInputDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public Guid? HomeCityId { get; set; }
        public string? HomeCityDisplay { get; set; }
        public List<Guid> LanguageIds { get; set; } = new();
        public string? Info { get; set; }
        public List<UserFactInputDto> Facts { get; set; } = new();
        public bool ShowGeneration { get; set; } = true;
    }


}
