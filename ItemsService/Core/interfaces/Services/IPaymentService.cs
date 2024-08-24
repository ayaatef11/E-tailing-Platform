using OrdersAndItemsService.Core.Entities.BasketEntites;

namespace OrdersAndItemsService.Core.interfaces.Services
{
    public interface IPaymentService
    {
        public  Task<Basket?> CreateOrUpdatePaymentIntent(string basketId);
    }
}
