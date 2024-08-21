﻿using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using OrdersAndItemsService.Core.Models;
using OrdersAndItemsService.Repository.IRepositories;
using OrdersAndItemsService.Services.Iservice;
using Stripe;
using System.Threading.Tasks;

namespace OrdersAndItemsService.Services.Service
{
    public class PaymentService(IGenericRepository<Item> repo, IBasketRepository basketRepo, IConfiguration configuration) : IpaymentService
    {
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = configuration["StripeSettings:SecretKey"];
            var basket = await basketRepo.GetBasketAsync(basketId);
            var shippingPrice = 0m;

            if (basket.DeliveryMethodId != 0)
            {
                var deliveryMethod = await repo.GetByIdAsync(basket.DeliveryMethodId);
                shippingPrice = deliveryMethod.Price;
            }

            foreach (var item in basket.Items)
            {
                var productItem = await repo.GetByIdAsync(item.Id);
                if (item.Price != productItem.Price)
                    item.Price = productItem.Price;
            }

            var service = new PaymentIntentService();
            PaymentIntent intent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingPrice * 100
                };

                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            await basketRepo.UpdateBasketAsync(basket);
            return basket;
        }
    }
}
