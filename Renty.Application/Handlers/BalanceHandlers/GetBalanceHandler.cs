using MediatR;
using Microsoft.AspNetCore.Identity;
using Renty.Application.Common;
using Renty.Application.DTOs.Balance;
using Renty.Application.Queries.Stripe;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.BalanceHandlers
{
    public class GetBalanceHandler : IRequestHandler<GetBalanceQuery, OperationResult<GetBalanceDto>>
    {
        private readonly IPaymentService _paymentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetBalanceHandler(
            IPaymentService paymentService,
            UserManager<ApplicationUser> userManager)
        {
            _paymentService = paymentService;
            _userManager = userManager;
        }
        public async Task<OperationResult<GetBalanceDto>> Handle(GetBalanceQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.CurrentUserId.ToString());

            if(user == null || string.IsNullOrEmpty(user.StripeAccountId))
            {
                return OperationResult<GetBalanceDto>.Fail("Stripe-аккаунт не настроен.");
            }

            var balance = await _paymentService.GetBalanceAsync(user.StripeAccountId,cancellationToken);

            return OperationResult<GetBalanceDto>.Success(new GetBalanceDto
            {
                Available = balance.AvailableInCents / 100m,
                Pending = balance.PendingInCents / 100m,
                Total = (balance.AvailableInCents + balance.PendingInCents) / 100m,
                Currency = balance.Currency.ToUpperInvariant()
            });
        }
    }
}
