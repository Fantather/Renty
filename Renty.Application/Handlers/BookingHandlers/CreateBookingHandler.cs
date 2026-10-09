using MediatR;
using Microsoft.AspNetCore.Identity;
using Renty.Application.Commands.BookingCommands;
using Renty.Application.Common;
using Renty.Application.DTOs.Booking;
using Renty.Application.Helpers;
using Renty.Domain.Enums;
using Renty.Domain.Interfaces;
using Renty.Domain.Models.Orders;
using Renty.Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Renty.Application.Handlers.BookingHandlers
{
    public class CreateBookingHandler : IRequestHandler<CreateBookingCommand, OperationResult<CreateBookingResponse>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IPaymentService _paymentService;
        private readonly PriceCalculatorService _priceCalculatorService;
        private readonly IPropertyRepository _propertyRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public CreateBookingHandler(
            IBookingRepository bookingRepository,
            IPaymentService paymentService,
            PriceCalculatorService priceCalculatorService,
            IPropertyRepository propertyRepository,
            UserManager<ApplicationUser> userManager)
        {
            _bookingRepository = bookingRepository;
            _paymentService = paymentService;
            _priceCalculatorService = priceCalculatorService;
            _propertyRepository = propertyRepository;
            _userManager = userManager;
        }
        public async Task<OperationResult<CreateBookingResponse>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository.GetPropertyWithDetailsAsync(request.PropertyId, cancellationToken);

            var result = _priceCalculatorService.Calculate(property, DateOnly.FromDateTime(request.CheckInDate), DateOnly.FromDateTime(request.CheckOutDate));

            if (!result.IsSuccess)
                return OperationResult<CreateBookingResponse>.Fail(result.Errors.ToArray());

            var isValide = await _bookingRepository.IsDateRangeAvailableAsync(property!.Id,request.CheckInDate,request.CheckOutDate,cancellationToken);

            if (!isValide)
                return OperationResult<CreateBookingResponse>.Fail("Выбранный диапазон дат уже забронирован");

            var booking = new Booking
            {
                PropertyId = property!.Id,
                CheckInDate = request.CheckInDate,
                CheckOutDate = request.CheckOutDate,
                GuestsCount = request.GuestsCount,
                PaymentMethod = request.PaymentMethod,
                Currency = property.Currency,
                UserId = request.CurrentUserId,
                TotalPrice = result.Data!
            };

            var owner = booking.Property.Host;

            string? clientSecret = null;

            if(request.PaymentMethod == PaymentMethodType.Card)
            {
                if (!owner.StripeOnboardingComplete)
                {
                    var status = await  _paymentService.GetConnectedAccountStatusAsync(owner.StripeAccountId!, cancellationToken);
                    
                    if(!status.CanReceiveTransfers)
                        return OperationResult<CreateBookingResponse>.Fail("Арендодатель не принимает оплату картой");

                    owner.StripeOnboardingComplete = status.CanReceiveTransfers;

                    await _userManager.UpdateAsync(owner);
                }
                    

                var amount = StripeAmountConverter.ToStripeAmount(booking.TotalPrice, booking.Currency);

                // Процент базоваой комиссии
                var fee = StripeAmountConverter.ToStripeAmount(booking.TotalPrice * 0.10m, booking.Currency);

                var intent = await _paymentService.CreatePaymentIntentAsync(booking.Id,amount,booking.Currency,owner.StripeAccountId!,fee,property.InstantBook,cancellationToken);

                booking.PaymentIntentId = intent.PaymentIntentId;
                clientSecret = intent.ClientSecret;
            }

            await _bookingRepository.AddAsync(booking, cancellationToken);
            return OperationResult<CreateBookingResponse>.Success(new CreateBookingResponse
            {
                BookingId = booking.Id,
                ClientSecret = clientSecret
            });
        }
    }
}
