using MediatR;
using Microsoft.AspNetCore.Identity;
using Renty.Application.Commands.LoginCommands;
using Renty.Application.Common;
using Renty.Application.DTOs.Login;
using Renty.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.LoginHandlers
{
    public class LoginHandler : IRequestHandler<LoginCommand, OperationResult<LoginResponse>>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginHandler(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<OperationResult<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Находим по почте, проверяем пароль с учетом хеширования 
            // Если все в порядке - ставит authentication cookie в HttpContext.Response
            var result = await _signInManager.PasswordSignInAsync(
                request.Email,
                request.Password,
                request.RememberMe,
                lockoutOnFailure: true);

            // Аккаунт заблокирован за слишком многих неудачных попыток входа
            if (result.IsLockedOut)
                return OperationResult<LoginResponse>.Fail("The account has been temporarily suspended due to multiple failed login attempts");

            // Почта не подтверждена
            if (result.IsNotAllowed)
                return OperationResult<LoginResponse>.Fail("Unable to log in. Please confirm your email before logging in");

            // Неверный email или пароль
            if (!result.Succeeded)
                return OperationResult<LoginResponse>.Fail("Incorrect email or password");

            // Куки установлены, возвращаем куда редирект
            return OperationResult<LoginResponse>.Success(new LoginResponse { ReturnUrl = request.ReturnUrl ?? "/" });
        }
    }
}
