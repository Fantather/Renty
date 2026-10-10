using Renty.Domain.ServiceModels.Stripe;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Domain.Interfaces
{
    public interface IPaymentService
    {
        // Онбординг арендодателя
        Task<string> CreateConnectedAccountAsync(string userId, string email, string countryCode, string firstName, string lastName, CancellationToken ct = default);
        Task<string> CreateOnboardingLinkAsync(string stripeAccountId, string returnUrl, string refreshUrl, CancellationToken ct = default);

        // Оплата
        Task<PaymentIntentResult> CreatePaymentIntentAsync(Guid bookingId, long amountInCents, string currency, string destinationAccountId, long applicationFeeInCents, bool autoBookingEnabled, CancellationToken ct = default);
        Task<PaymentIntentResult> CreatePlatformPaymentIntentAsync(Guid bookingId, long amountInCents, string currency, CancellationToken ct = default);
        Task<PaymentIntentResult> GetPaymentIntentAsync(string paymentIntentId, CancellationToken ct = default);
        Task CapturePaymentAsync(string paymentIntentId, CancellationToken ct = default);
        Task CancelPaymentAsync(string paymentIntentId, CancellationToken ct = default);

        // Баланс/выплаты
        Task<BalanceDto> GetBalanceAsync(string stripeAccountId, CancellationToken ct = default);
        Task<string> CreatePayoutAsync(string stripeAccountId, long amountInCents, string currency, CancellationToken ct = default);

        // Статус аккаунта
        Task<ConnectedAccountStatus> GetConnectedAccountStatusAsync(string stripeAccountId, CancellationToken ct = default);

        Task RefundPaymentAsync(string paymentIntentId, CancellationToken ct = default);
    }
}
