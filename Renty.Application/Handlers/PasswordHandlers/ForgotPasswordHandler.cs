using MediatR;
using Microsoft.AspNetCore.Identity;
using Renty.Application.Commands.PasswordCommands;
using Renty.Application.Common;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.User;
using Renty.Domain.ServiceModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Renty.Application.Handlers.PasswordHandlers
{
    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, OperationResult<bool>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        public ForgotPasswordHandler(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }
        public async Task<OperationResult<bool>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            // Если пользователь не найден или почта не подтвержденна - выход с "успехом"
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
                return OperationResult<bool>.Success(true);

            // Генерируем токен сброса
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);

            var resetLink = $"{request.ResetPasswordBaseUrl}?userId={user.Id}&token={encodedToken}";

            var message = new EmailMessage(
                new string[] { user.Email },
                "Reset password in Renty.",
                $"To reset your password, follow this link: \n\n<a href='{resetLink}'>Reset password</a>"
            );

            await _emailSender.SendEmailAsync(message);

            return OperationResult<bool>.Success(true);
        }
    }
}
