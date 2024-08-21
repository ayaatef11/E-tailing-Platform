using OrdersAndItemsService.Core.Models;

namespace OrdersAndItemsService.Repository.IRepositories
{
    public interface IitemRepository
    {
        Task<Item> getItemByIdAsync(int id);
        Task<IReadOnlyList<Item>> GetItemsAsync();
    }
}
