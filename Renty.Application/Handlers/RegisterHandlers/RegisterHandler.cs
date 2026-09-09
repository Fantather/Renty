using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Renty.Application.Commands.RegisterCommands;
using Renty.Application.Common;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.User;
using Renty.Domain.ServiceModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Renty.Application.Handlers.RegisterHandlers
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, OperationResult<bool>>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public RegisterHandler(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }
        public async Task<OperationResult<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            // Пользователь с такой почтой уже зарегистрирован
            if (existingUser != null)
                return OperationResult<bool>.Fail("A user with this email is already registered");

            var user = new ApplicationUser
            {
                UserName = request.UserName,
                Email = request.Email,
                EmailConfirmed = false
            };

            var createResult = await _userManager.CreateAsync(user, request.Password);

            if (!createResult.Succeeded)
            {
                return OperationResult<bool>.Fail(createResult.Errors.Select(e => e.Description).ToArray());
            }

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


            return OperationResult<bool>.Success(true);
        }
    }
}
