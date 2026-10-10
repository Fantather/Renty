using MediatR;
using Microsoft.AspNetCore.Identity;
using Renty.Application.Commands.RegisterCommands;
using Renty.Application.Common;
using Renty.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Renty.Application.Handlers.RegisterHandlers
{
    public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, OperationResult<bool>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public ConfirmEmailHandler(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<OperationResult<bool>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user == null)
                return OperationResult<bool>.Fail("User not found");

            if (user.EmailConfirmed)
                return OperationResult<bool>.Success(false);

            var result = await _userManager.ConfirmEmailAsync(user,request.Token);

            if (!result.Succeeded)
            {
                return OperationResult<bool>.Fail(result.Errors.Select(e => e.Description).ToArray());
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            return OperationResult<bool>.Success(true);
        }
    }
}
