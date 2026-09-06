using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Renty.Application.Commands.LoginCommands;
using Renty.Application.Commands.LogoutCommands;
using Renty.Application.DTOs.Login;
using Renty.Web.Models;

namespace Renty.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
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

        //[HttpPost]
        //public IActionResult Login(LoginViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        // TODO: сейчас модалка не умеет показывать серверные ошибки — просто возвращаемся назад.
        //        // Понадобится доработать, когда подключим реальный вход через Identity.
        //        return RedirectToAction("Index", "Home");
        //    }

        //    // TODO: настоящий вход (проверка пароля, SignInManager), когда подключим Identity.
        //    return RedirectToAction("Index", "Home");
        //}
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDtoRequest dto)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ReturnUrl"] = dto.ReturnUrl;
                return View(dto);
            }

            var result = await _mediator.Send(new LoginCommand(dto.Email, dto.Password, dto.RememberMe, dto.ReturnUrl));

            if (!result.IsSuccess)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error);

                ViewData["ReturnUrl"] = dto.ReturnUrl;
                return View(dto);
            }

            return LocalRedirect(result.Data.ReturnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var principal = HttpContext.User;
            await _mediator.Send(new LogoutCommand(principal));
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginsList()
        {
            var result = await _mediator.Send(new GetExternalLoginsCommand());

            if (!result.IsSuccess)
                return View("Error", result);

            foreach (var login in result.Data)
                login.Url = Url.Action("ExternalLogin", "Account", new { provider = login.Name });

            return View(result.Data);
        }
        [HttpGet]
        public async Task<IActionResult> ExternalLogin(ExternalLoginRequest dto)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { ReturnUrl = dto.ReturnUrl });

            var result = await _mediator.Send(new ExternalLoginCommand(dto.Provider, redirectUrl));

            if (!result.IsSuccess)
                return RedirectToAction(nameof(Login));

            return Challenge(result.Data.Properties, result.Data.Provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(ExternalLoginCallbackRequest dto)
        {
            string returnUrl = dto.ReturnUrl ?? Url.Content("~/");

            var result = await _mediator.Send(new ExternalLoginCallbackCommand(returnUrl, dto.RemoteError));

            if (!result.IsSuccess)
            {
                TempData["Error"] = string.Join(", ", result.Errors);
                return RedirectToAction(nameof(Login));
            }

            return LocalRedirect(result.Data.ReturnUrl);
        }
    }
}
