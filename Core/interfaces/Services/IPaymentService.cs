using Core.Entities.BasketEntites;

namespace Core.interfaces.Services
{
    public interface IPaymentService
    {
        public Task<Basket?> CreateOrUpdatePaymentIntent(string basketId);
    }
}
