using System.ComponentModel.DataAnnotations;

namespace Renty.Web.Models.InputModels.Users
{
    public class UploadAvatarInputModel
    {
        [Required]
        public IFormFile File { get; set; } = null!;
    }
}
