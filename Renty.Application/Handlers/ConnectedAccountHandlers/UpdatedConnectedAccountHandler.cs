using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Renty.Application.Commands.ConnectedAccountCommands;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.ConnectedAccountHandlers
{
    public class UpdatedConnectedAccountHandler : IRequestHandler<UpdatedConnectedAccountCommand, Unit>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UpdatedConnectedAccountHandler(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Unit> Handle(UpdatedConnectedAccountCommand request, CancellationToken cancellationToken)
        {
            
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.StripeAccountId == request.ConnectedAccountId,cancellationToken);

            if(user == null)
            {
                return Unit.Value;
            }

            user.StripeOnboardingComplete = request.CanReceiveTransfers;

            await _userManager.UpdateAsync(user);

            return Unit.Value;
        }
    }
}
