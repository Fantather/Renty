using Renty.Domain.Models.LookupsTables;

namespace Renty.Web.Models.InputModels.Users
{
    public class UserFactInputModel
    {
        public UserFactTypeEnum Type { get; set; }
        public string Value { get; set; } = string.Empty;
        public string IconName { get; set; } = string.Empty;
    }
}
