using System.ComponentModel.DataAnnotations;

namespace Renty.Web.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введите имя")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите фамилию")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите дату рождения")]
        public int? BirthDay { get; set; }

        [Required(ErrorMessage = "Укажите дату рождения")]
        public int? BirthMonth { get; set; }

        [Required(ErrorMessage = "Укажите дату рождения")]
        public int? BirthYear { get; set; }

        [Required(ErrorMessage = "Введите email")]
        [EmailAddress(ErrorMessage = "Некорректный email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите пароль")]
        [MinLength(8, ErrorMessage = "Пароль должен быть не короче 8 символов")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Подтвердите пароль")]
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
