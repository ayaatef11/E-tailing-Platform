using OrdersAndItemsService.Core.Models;
using OrdersAndItemsService.Repository.Data;
using OrdersAndItemsService.Repository.IRepositories;

namespace OrdersAndItemsService.Repository.Repositories
{
    public class ItemRepository(AppDbContext _context) : IitemRepository
    {
        public async Task<Item> getItemByIdAsync(int id)
        {
            return await _context.Items.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IReadOnlyList<Item>> GetItemsAsync()
        {
            return await _context.Items.ToListAsync();
        }


    }
}
