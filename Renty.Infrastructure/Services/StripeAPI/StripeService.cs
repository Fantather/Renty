using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.User;
using Renty.Domain.ServiceModels.Stripe;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Infrastructure.Services.StripeAPI
{
    public class StripeService : IPaymentService
    {
        private readonly StripeClient _client;
        public StripeService(IOptions<StripeOptions> options,
            UserManager<ApplicationUser> userManager)
        {
            _client = new StripeClient(options.Value.StKey);
        }
        /// <summary>
        /// Отмена оплаты
        /// </summary>
        /// <param name="paymentIntentId">Идентификатор платежа</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns></returns>
        public async Task CancelPaymentAsync(string paymentIntentId, CancellationToken ct=default)
        {
            var service = new PaymentIntentService(_client);
            await service.CancelAsync(paymentIntentId, cancellationToken: ct);
        }
        /// <summary>
        /// Списать средства
        /// </summary>
        /// <param name="paymentIntentId">Идентификатор платежа</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns></returns>
        public async Task CapturePaymentAsync(string paymentIntentId, CancellationToken ct = default)
        {
            var service = new PaymentIntentService(_client);
            await service.CaptureAsync(paymentIntentId, cancellationToken: ct);
        }
        /// <summary>
        /// Создать аккаунт
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Идентификатор connectedAccount</returns>
        public async Task<string> CreateConnectedAccountAsync(string userId, string email, string countryCode, string firstName, string lastName, CancellationToken ct = default)
        {

            var account = await new AccountService(_client).CreateAsync(new AccountCreateOptions
            {
                Type = "express",
                Country = countryCode,
                Email = email,
                BusinessType = "individual",
                Individual = new AccountIndividualOptions { FirstName = firstName, LastName = lastName, Email = email },
                Capabilities = new AccountCapabilitiesOptions
                {
                    Transfers = new AccountCapabilitiesTransfersOptions { Requested = true }
                },
                Metadata = new Dictionary<string, string> { ["rentyUserId"] = userId }
            }, cancellationToken: ct);

            return account.Id;
        }
        /// <summary>
        /// Создать ссылку для вывода средств
        /// </summary>
        /// <param name="stripeAccountId">Идентификатор аккаунта</param>
        /// <param name="returnUrl">URL-адрес, на который пользователь будет перенаправлен после завершения связанного процесса</param>
        /// <param name="refreshUrl">URL-адрес, на который пользователь будет перенаправлен, если AccountLink истек, был использован или по какой-либо другой причине недействителен</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Ссылка для вывода средств</returns>
        public async Task<string> CreateOnboardingLinkAsync(string stripeAccountId, string returnUrl, string refreshUrl, CancellationToken ct = default)
        {
            var link = await new AccountLinkService(_client).CreateAsync(new AccountLinkCreateOptions
            {
                Account = stripeAccountId,
                RefreshUrl = refreshUrl,
                ReturnUrl = returnUrl,
                Type = "account_onboarding"
            }, cancellationToken: ct);
            return link.Url;
        }
        /// <summary>
        /// Создание оплаты
        /// </summary>
        /// <param name="bookingId">Идентификатор заказа</param>
        /// <param name="amountInCents">Сумма которая будет взиматься в центах</param>
        /// <param name="currency">Трехбуквенный код валюты ISO</param>
        /// <param name="destinationAccountId">Индефикатор аккаунта получателя</param>
        /// <param name="applicationFeeInCents">Сумма комиссии за обработку платежа</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Клиентский секрет (client secret) для данного объекта PaymentIntent. Используется на стороне клиента для получения данных с помощью публичного ключа (publishable key). Этот секрет позволяет завершить платеж на стороне фронтенда.</returns>
        public async Task<PaymentIntentResult> CreatePaymentIntentAsync(Guid bookingId, long amountInCents, string currency, string destinationAccountId, long applicationFeeInCents, bool autoBookingEnabled, CancellationToken ct = default)
        {

            var service = new PaymentIntentService(_client);
            var intent = await service.CreateAsync(new PaymentIntentCreateOptions
            {
                Amount = amountInCents,
                Currency = currency,
                CaptureMethod = autoBookingEnabled ? "automatic" : "manual",
                TransferData = new PaymentIntentTransferDataOptions
                {
                    Destination = destinationAccountId
                },
                ApplicationFeeAmount = applicationFeeInCents,
                Metadata = new Dictionary<string, string> { ["bookingId"] = bookingId.ToString()}
            }, cancellationToken: ct);

            return new PaymentIntentResult
            {
                PaymentIntentId = intent.Id,
                ClientSecret = intent.ClientSecret
            };
        }
        /// <summary>
        /// Вывод средств с кошелька
        /// </summary>
        /// <param name="stripeAccountId">Идентификатор аккаунта</param>
        /// <param name="amountInCents">Сумма которая будет взиматься в центах</param>
        /// <param name="currency">Трехбуквенный код валюты ISO</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Идентификатор выплаты</returns>
        public async Task<string> CreatePayoutAsync(string stripeAccountId, long amountInCents, string currency, CancellationToken ct = default)
        {
            var service = new PayoutService(_client);
            var payout = await service.CreateAsync(
                new PayoutCreateOptions
                {
                    Amount = amountInCents,
                    Currency = currency
                },
                new RequestOptions
                {
                    StripeAccount = stripeAccountId
                },
            ct);

            return payout.Id;
        }
        /// <summary>
        /// Получить баланс аккаунта
        /// </summary>
        /// <param name="stripeAccountId">Идентификатор аккаунта</param>
        /// <param name="ct">Токен отмены</param>
        /// <returns>Возвращает средства на балансе которые доступных к выводу и задержаны на счету</returns>
        public async Task<BalanceDto> GetBalanceAsync(string stripeAccountId, CancellationToken ct = default)
        {
            var service = new BalanceService(_client);
            var balance = await service.GetAsync(new RequestOptions
            {
                StripeAccount = stripeAccountId
            }, ct);

            var available = balance.Available.FirstOrDefault();
            var pending = balance.Pending.FirstOrDefault(p => p.Currency == available?.Currency)
                          ?? balance.Pending.FirstOrDefault();

            return new BalanceDto
            {
                AvailableInCents = available?.Amount ?? 0,
                PendingInCents = pending?.Amount ?? 0,
                Currency = available?.Currency ?? pending?.Currency ?? "usd"
            };
        }

        public async Task<ConnectedAccountStatus> GetConnectedAccountStatusAsync(string stripeAccountId, CancellationToken ct = default)
        {
            var account = await new AccountService(_client).GetAsync(stripeAccountId, cancellationToken: ct);

            return new ConnectedAccountStatus
            {
                CanReceiveTransfers = account.Capabilities?.Transfers == "active",
                HasOutstandingRequirements = account.Requirements?.CurrentlyDue?.Count > 0
            };
        }

        public async Task RefundPaymentAsync(string paymentIntentId, CancellationToken ct = default)
        {
            var service = new RefundService(_client);
            await service.CreateAsync(new RefundCreateOptions
            {
                PaymentIntent = paymentIntentId,
                ReverseTransfer = true,        // забрать деньги с баланса арендодателя
                RefundApplicationFee = true    // вернуть и комиссию платформы
            }, cancellationToken: ct);
        }
    }
}
