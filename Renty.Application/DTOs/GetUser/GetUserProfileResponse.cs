using Renty.Application.DTOs.GetReviews;

namespace Renty.Application.DTOs.GetUser
{
    public class GetUserProfileResponse
    {
        public Guid UserId { get; set; }
        public bool IsOwner { get; set; }
        public string AvatarUrl { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public bool IsSuperHost { get; set; }
        public decimal Rating { get; set; }
        public int ReviewsCount { get; set; }
        public int MonthsOnPlatform { get; set; }
        public bool IsVerified { get; set; }
        public string HomeCity { get; set; } = string.Empty;
        public string HomeCountry { get; set; } = string.Empty;
        public List<string> Languages { get; set; } = new();
        public string? Info { get; set; }
        public List<UserFactDto> Facts { get; set; } = new();
        public List<ReviewDto> Reviews { get; set; } = new();
    }



}
