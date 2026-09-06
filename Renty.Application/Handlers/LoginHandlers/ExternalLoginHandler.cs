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
    public class ExternalLoginHandler : IRequestHandler<ExternalLoginCommand, OperationResult<ExternalLoginChallenge>>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ExternalLoginHandler(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }
        public async Task<OperationResult<ExternalLoginChallenge>> Handle(ExternalLoginCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Provider))
                return OperationResult<ExternalLoginChallenge>.Fail("Provider is null or empty");

            // Получаем настроенную инструкцию для authentication middleware
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(request.Provider, request.RedirectUrl);

            return OperationResult<ExternalLoginChallenge>.Success(new ExternalLoginChallenge { Provider = request.Provider, Properties = properties });
        }
    }
}
