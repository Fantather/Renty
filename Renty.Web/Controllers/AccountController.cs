using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Renty.Application.Commands.LoginCommands;
using Renty.Application.Commands.LogoutCommands;
using Renty.Application.Commands.PasswordCommands;
using Renty.Application.Commands.RegisterCommands;
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

        //public IActionResult Register()
        //{
        //    return View(new RegisterViewModel());
        //}

        //[HttpPost]
        //public IActionResult Register(RegisterViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // TODO: настоящая регистрация (создание пользователя, хэш пароля), когда подключим Identity.
        //    return RedirectToAction("Index", "Home");
        //}

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

        #region Register

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Полный абсолютный адрес
            var confirmBaseUrl = Url.Action(nameof(ConfirmEmail), "Account", null, Request.Scheme)!;

            var result = await _mediator.Send(new RegisterCommand(model.Name, model.Email, model.Password, confirmBaseUrl));

            if (!result.IsSuccess)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return View(model);
            }

            return RedirectToAction(nameof(RegisterConfirmation), new {email = model.Email});
        }

        [HttpPost]
        public async Task<IActionResult> SendConfirmationEmail(string email)
        {
            // Полный абсолютный адрес
            var confirmBaseUrl = Url.Action(nameof(ConfirmEmail), "Account", null, Request.Scheme)!;

            await _mediator.Send(new SendConfirmationEmailCommand(email, confirmBaseUrl));

            return RedirectToAction(nameof(RegisterConfirmation), new {email = email});
        }

        // Страница: "Письмо отправлено, проверьте почту"
        [HttpGet]
        public IActionResult RegisterConfirmation(string email) 
        {
            ViewData["email"] = email;
            return View(); 
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return LocalRedirect(nameof(Login));

            var result = await _mediator.Send(new ConfirmEmailCommand(userId, token));

            if (!result.IsSuccess)
            {
                // Ошибка подтверждения
                ViewData["Error"] = string.Join(", ", result.Errors);
                ViewData["IsConfirm"] = result.IsSuccess;
                return View(nameof(ConfirmEmail));
            }

            // Почта подтверждена
            ViewData["IsConfirm"] = result.IsSuccess;
            return View(nameof(ConfirmEmail));
        }

        #endregion

        #region Login

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, error = string.Join(" ", modelErrors) });
            }

            var result = await _mediator.Send(new LoginCommand(model.Email, model.Password, ReturnUrl: returnUrl));

            if (!result.IsSuccess)
            {
                return Json(new { success = false, error = string.Join(" ", result.Errors) });
            }

            return Json(new { success = true, returnUrl = result.Data.ReturnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var principal = HttpContext.User;
            await _mediator.Send(new LogoutCommand(principal));
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region ExternalLogin

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

        #endregion

        #region Password

        [HttpGet]
        public IActionResult ForgotPassword() => View(new ForgotPasswordViewModel());

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var resetBaseUrl = Url.Action(nameof(ResetPassword), "Account", null, Request.Scheme)!;

            await _mediator.Send(new ForgotPasswordCommand(model.Email, resetBaseUrl));

            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        // Страница "проверьте почту"
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation() => View();


        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return RedirectToAction(nameof(Login));

            var model = new ResetPasswordViewModel
            {
                UserId = userId,
                Token = token,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _mediator.Send(new ResetPasswordCommand(model.UserId, model.Token, model.NewPassword));

            if (!result.IsSuccess)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return View(model);
            }

            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        // Удачное изменение пароля
        [HttpGet]
        public IActionResult ResetPasswordConfirmation() => View();

        #endregion
    }
}
