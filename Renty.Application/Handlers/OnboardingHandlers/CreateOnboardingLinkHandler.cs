using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Renty.Application.Commands.OnbordingCommands;
using Renty.Application.Common;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.OnbordingHandlers
{
    public class CreateOnboardingLinkHandler : IRequestHandler<CreateOnboardingLinkCommand, OperationResult<string>>
    {
        private readonly IPaymentService _paymentService;
        private readonly UserManager<ApplicationUser> _userManager;
        public CreateOnboardingLinkHandler(
            IPaymentService paymentService,
            UserManager<ApplicationUser> userManager)
        {
            _paymentService = paymentService;
            _userManager = userManager;
        }
        public async Task<OperationResult<string>> Handle(CreateOnboardingLinkCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.CurrentUserId.ToString());

            if (user == null)
            {
                return OperationResult<string>.Fail("User not found");
            }

            if (string.IsNullOrEmpty(user.StripeAccountId))
            {
                // те же проверки, что в CreateConnectedAccountHandler
                if (user.HomeCountryId == null)
                    return OperationResult<string>.Fail("User's country not selected");
                if (string.IsNullOrWhiteSpace(user.FirstName) || string.IsNullOrWhiteSpace(user.LastName))
                    return OperationResult<string>.Fail("FirstName or LastName is empty");

                user.StripeAccountId = await _paymentService.CreateConnectedAccountAsync(
                    user.Id.ToString(), user.Email!, user.HomeCountry!.CountryCode, user.FirstName, user.LastName, cancellationToken);
                await _userManager.UpdateAsync(user);
            }

            var url = await _paymentService.CreateOnboardingLinkAsync(user.StripeAccountId, request.ReturnUrl, request.RefreshUrl, cancellationToken);

            return OperationResult<string>.Success(url);
        }
    }
}
