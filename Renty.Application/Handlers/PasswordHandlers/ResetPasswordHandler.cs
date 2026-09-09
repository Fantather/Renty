using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using Renty.Application.Commands.PasswordCommands;
using Renty.Application.Common;
using Renty.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.PasswordHandlers
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, OperationResult<bool>>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<OperationResult<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
            {
                return OperationResult<bool>.Fail("Failed to reset the password. The link is invalid or expired.");
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

            if (!result.Succeeded)
            {
                return OperationResult<bool>.Fail(result.Errors.Select(e => e.Description).ToArray());
            }

            return OperationResult<bool>.Success(true);
        }
    }
}
