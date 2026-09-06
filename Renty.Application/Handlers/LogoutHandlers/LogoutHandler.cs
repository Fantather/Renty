using MediatR;
using Microsoft.AspNetCore.Identity;
using Renty.Application.Commands.LogoutCommands;
using Renty.Application.Common;
using Renty.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.LogoutHandlers
{
    public class LogoutHandler : IRequestHandler<LogoutCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public LogoutHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(request.Principal);

            if(user != null)
            {
                var existingClaims = await _userManager.GetClaimsAsync(user);
                await _userManager.RemoveClaimsAsync(user, existingClaims);
            }

            await _signInManager.SignOutAsync();

            return;
        }
    }
}
