using OrdersAndItemsService.Models;

namespace OrdersAndItemsService.Iservice
{
    public interface IpaymentService
    {
        public Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId);
    }
}
