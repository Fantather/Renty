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
    public class GetExternalLoginsHandler : IRequestHandler<GetExternalLoginsCommand, OperationResult<IEnumerable<ExternalLoginResponse>>>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public GetExternalLoginsHandler(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }
        public async Task<OperationResult<IEnumerable<ExternalLoginResponse>>> Handle(GetExternalLoginsCommand request, CancellationToken cancellationToken)
        {
            // Находим все зарегистрированные схемы внешних входов в систему
            var schemes = await _signInManager.GetExternalAuthenticationSchemesAsync();

            var externalLogins = schemes.Select(s => new ExternalLoginResponse
            {
                Name = s.Name,
                DisplayName = s.DisplayName
            });

            return OperationResult<IEnumerable<ExternalLoginResponse>>.Success(externalLogins);
        }
    }
}
