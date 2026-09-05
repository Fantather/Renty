using Microsoft.AspNetCore.Mvc;
using Renty.Web.Models;

namespace Renty.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // TODO: настоящая регистрация (создание пользователя, хэш пароля), когда подключим Identity.
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // TODO: сейчас модалка не умеет показывать серверные ошибки — просто возвращаемся назад.
                // Понадобится доработать, когда подключим реальный вход через Identity.
                return RedirectToAction("Index", "Home");
            }

            // TODO: настоящий вход (проверка пароля, SignInManager), когда подключим Identity.
            return RedirectToAction("Index", "Home");
        }
    }
}
