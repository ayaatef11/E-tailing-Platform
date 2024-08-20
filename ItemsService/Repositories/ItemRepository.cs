using OrdersAndItemsService.IRepositories;
using WebApplication1.Data;

namespace OrdersAndItemsService.Repositories
{
    public class ItemRepository(AppDbContext _context) : IitemRepository
    {
       public async Task<Item>  getItemByIdAsync(int id)
        {
            return await  _context.Items.FirstOrDefaultAsync(p => p.Id == id);
        }

       public async Task<IReadOnlyList<Item>> GetItemsAsync()
        {
            return await _context.Items.ToListAsync();
        }

     
    }
}
