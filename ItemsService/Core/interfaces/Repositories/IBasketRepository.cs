
using OrdersAndItemsService.Core.Models.BasketEntites;

namespace OrdersAndItemsService.Core.interfaces.Repositories
{
    public interface IBasketRepository
    {

        Task<Basket?> CreateOrUpdateBasketAsync(Basket basket);
        Task<Basket?> GetBasketAsync(string basketId);
        Task<bool> DeleteBasketAsync(string basketId);
    }
}
