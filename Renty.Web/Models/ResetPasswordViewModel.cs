using System.ComponentModel.DataAnnotations;

namespace Renty.Web.Models
{
    public class ResetPasswordViewModel
    {
        public string UserId { get; set; } = null!;
        public string Token { get; set; } = null!;

        [Required(ErrorMessage = "Введите пароль")]
        [MinLength(8, ErrorMessage = "Пароль должен быть не короче 8 символов")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Подтвердите пароль")]
        [Compare(nameof(NewPassword), ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
