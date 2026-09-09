using MediatR;
using Microsoft.AspNetCore.Identity;
using Renty.Application.Commands.RegisterCommands;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.User;
using Renty.Domain.ServiceModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Renty.Application.Handlers.RegisterHandlers
{
    public class SendConfirmationEmailHandler : IRequestHandler<SendConfirmationEmailCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        public SendConfirmationEmailHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
        public async Task Handle(SendConfirmationEmailCommand request, CancellationToken cancellationToken)
        {

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null) return;

            // Генерируем токен подтверждения email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            // Закодируем токен для безопасной передачи в query
            var encodedToken = WebUtility.UrlEncode(token);

            // Строим абсолютную ссылку
            var confirmationLink = $"{request.ConfirmEmailBaseUrl}?userId={user.Id}&token={encodedToken}";

            // Формируем сообщение
            var message = new EmailMessage(
                new string[] { request.Email },
                "Email verification in Renty.",
                $"Click this link to confirm your email, or ignore this message \n\n<a href='{confirmationLink}'>Confirm email</a>"
                );

            await _emailSender.SendEmailAsync(message);
        }
    }
}
