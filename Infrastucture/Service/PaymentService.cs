using Application.Dtos;
using Application.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Service
{
	public class PaymentService:IPaymentService
	{
		private readonly IPaymentRepository _paymentRepo;
		private readonly IConfiguration _config;
		public PaymentService(IPaymentRepository paymentRepo, IConfiguration config)
		{
			_paymentRepo = paymentRepo;
			_config = config;
		}
		public async Task<CheckoutResponseDto> CreateCheckoutSessionAsync(CreateCheckoutDto dto, int userId)
		{
			var lineItems = dto.Items.Select(item => new SessionLineItemOptions
			{
				PriceData = new SessionLineItemPriceDataOptions
				{
					Currency = "usd",
					UnitAmount = (long)(item.Price * 100),
					ProductData = new SessionLineItemPriceDataProductDataOptions
					{
						Name = item.Name
					}
				},
				Quantity = item.Quantity
			}).ToList();

			var options = new SessionCreateOptions
			{
				PaymentMethodTypes = new List<string> { "card" },
				LineItems = lineItems,
				Mode = "payment",
				SuccessUrl = _config["Stripe:SuccessUrl"],
				CancelUrl = _config["Stripe:CancelUrl"]
			};

			var service = new SessionService();
			var session = await service.CreateAsync(options);

			var payment = new Payment
			{
				StripeSessionId = session.Id,
				UserId = userId,
				TotalAmount = dto.Items.Sum(i => i.Price * i.Quantity),
				Items = dto.Items.Select(i => new PaymentItem
				{
					Name = i.Name,
					Price = i.Price,
					Quantity = i.Quantity
				}).ToList()
			};

			await _paymentRepo.AddAsync(payment);
			await _paymentRepo.SaveChangesAsync();

			return new CheckoutResponseDto
			{
				CheckoutUrl = session.Url,
				SessionId = session.Id
			};
		}
	}
}
