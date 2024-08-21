using OrdersAndItemsService.Core.Models;

namespace OrdersAndItemsService.Services.Iservice
{
    public interface IpaymentService
    {
        public Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId);
    }
}
