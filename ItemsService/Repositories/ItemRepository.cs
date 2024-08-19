using OrdersAndItemsService.IRepositories;
using WebApplication1.Data;

namespace OrdersAndItemsService.Repositories
{
    public class ItemRepository(AppDbContext _context) : IitemRepository
    {
       public async Task<Item>  getItemByIdAsync(int id)
        {
            return  _context.Items.FirstOrDefault(p => p.Id == id);
        }

        async Task<IReadOnlyList<Item>> GetItemsAsync()
        {
            return await _context.Items.ToListAsync();
        }

        Task<IReadOnlyList<Item>> IitemRepository.GetItemsAsync()/////////////////////////////not correct 
        {
            throw new NotImplementedException();
        }
    }
}
