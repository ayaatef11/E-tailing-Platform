using OrdersAndItemsService.IRepositories;
using WebApplication1.Data;

namespace OrdersAndItemsService.Repositories
{
    public class ItemRepository(AppDbContext _context) : IitemRepository
    {
       public async Task<Item> IitemRepository.getItemByIdAsync(int id)
        {
            return await _context.Items.FirstOrDefault(p => p.Id == id);
        }

        Task<IReadOnlyList<Item>> IitemRepository.GetItemsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
