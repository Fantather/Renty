using MediatR;
using Microsoft.AspNetCore.Identity;
using Renty.Application.Commands.CategoryCommands;
using Renty.Application.Commands.StripeAccountCommands;
using Renty.Application.Common;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.StripeAccountHandlers
{
    public class CreateConnectedAccountHandler : IRequestHandler<CreateConnectedAccountCommand, OperationResult<Unit>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPaymentService _paymentService;
        public CreateConnectedAccountHandler(
        UserManager<ApplicationUser> userManager,
        IPaymentService paymentService)
        {
            _userManager = userManager;
            _paymentService = paymentService;
        }
        public async Task<OperationResult<Unit>> Handle(CreateConnectedAccountCommand request, CancellationToken cancellationToken)
        {

            var user = await _userManager.FindByIdAsync(request.CurrentUserId.ToString());
            if (user == null)
                return OperationResult<Unit>.Fail("User not found");

            if (user.HomeCountryId == null)
                return OperationResult<Unit>.Fail("User's country not selected");

            if (string.IsNullOrWhiteSpace(user.FirstName) || string.IsNullOrWhiteSpace(user.LastName))
                return OperationResult<Unit>.Fail("FirstName or LastName is empty");

            var accountId =await _paymentService.CreateConnectedAccountAsync(user.Id.ToString(), user.Email!, user.HomeCountry!.CountryCode, user.FirstName, user.LastName, cancellationToken);

            user.StripeAccountId = accountId;
            user.StripeOnboardingComplete = false;

            await _userManager.UpdateAsync(user);

            return OperationResult<Unit>.Success(Unit.Value);
        }
    }
}
