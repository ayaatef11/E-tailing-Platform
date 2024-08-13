using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using OrdersAndItemsService.IRepositories;
using OrdersAndItemsService.Iservice;
using OrdersAndItemsService.Models;
using Stripe;
using System.Threading.Tasks;

namespace OrdersAndItemsService.Service
{
    public class PaymentService : IpaymentService
    {
        private readonly IRepository<Item> _repo;
        private readonly IBasketRepository _basketRepo;
        private readonly IConfiguration _configuration;

        public PaymentService(IRepository<Item> repo, IBasketRepository basketRepo, IConfiguration configuration)
        {
            _repo = repo;
            _basketRepo = basketRepo;
            _configuration = configuration;
        }

        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            var basket = await _basketRepo.GetBasketAsync(basketId);
            var shippingPrice =0m;

            if (basket.DeliveryMethodId!=0)
            {
                var deliveryMethod = await _repo.GetByIdAsync((int)basket.DeliveryMethodId);
                shippingPrice = deliveryMethod.Price;
            }

            foreach (var item in basket.Items)
            {
                var productItem = await _repo.GetByIdAsync(item.Id);
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

            await _basketRepo.UpdateBasketAsync(basket);
            return basket;
        }
    }
}
