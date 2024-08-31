
using Core.Entities.BasketEntites;

namespace Core.interfaces.Repositories
{
    public interface IBasketRepository
    {

        Task<Basket?> CreateOrUpdateBasketAsync(Basket basket);
        Task<Basket?> GetBasketAsync(string basketId);
        Task<bool> DeleteBasketAsync(string basketId);
    }
}
