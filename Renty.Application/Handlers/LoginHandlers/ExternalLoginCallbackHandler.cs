using MediatR;
using Microsoft.AspNetCore.Identity;
using Renty.Application.Commands.LoginCommands;
using Renty.Application.Common;
using Renty.Application.DTOs.Login;
using Renty.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Renty.Application.Handlers.LoginHandlers
{
    public class ExternalLoginCallbackHandler : IRequestHandler<ExternalLoginCallbackCommand, OperationResult<ExternalLoginCallbackResponse>>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public ExternalLoginCallbackHandler(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<OperationResult<ExternalLoginCallbackResponse>> Handle(ExternalLoginCallbackCommand request, CancellationToken cancellationToken)
        {
            // Проверка ошибки от внешнего провайдера (Google)
            // Если пользователь отменил вход или произошла ошибка
            if (request.RemoteError != null)
                return OperationResult<ExternalLoginCallbackResponse>.Fail($"Error from external provider: {request.RemoteError}");

            // Получаем информацию о внешнем логине
            // Внутри: provider (Google), providerKey (уникальный ID пользователя в Google), claims
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if(info == null)
                return OperationResult<ExternalLoginCallbackResponse>.Fail("Error loading external login information");

            // Попопытка залогинить пользователя по внешнему провайдеру 
            // Если пользователь уже входил через Google раньше — он будет найден по ProviderKey
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true);

            ApplicationUser user;

            // Если пользователь уже существует и связан с Google 
            // Получаем его из базы
            if (signInResult.Succeeded)
            {
                user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            }

            // Если новый пользователь - нужно создать аккаунт
            else
            {
                // Достаём email из claims, который пришёл от Google

                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                // Без email мы не можем создать пользователя
                if(email == null)
                    return OperationResult<ExternalLoginCallbackResponse>.Fail("Email claim not received from Google");

                // Проверка существует ли пользователь в базе (зарегистрировался обычным способом)
                user = await _userManager.FindByEmailAsync(email);

                // Если нет создаем нового пользователя
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        Email = email,
                        EmailConfirmed = true
                    };

                    await _userManager.CreateAsync(user);
                }

                // Связываем пользователя с Google логином
                // Это сохраняет Provider + ProviderKey в AspNetUserLogins
                await _userManager.AddLoginAsync(user, info);

                // После создания привязки логиним пользователя вручную
                await _signInManager.SignInAsync(user, isPersistent: true);
            }

            return OperationResult<ExternalLoginCallbackResponse>.Success(new ExternalLoginCallbackResponse { ReturnUrl = request.ReturnUrl ?? "/" });
            
        }
    }
}
