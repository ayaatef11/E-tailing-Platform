namespace OrdersAndItemsService.IRepositories
{
    public interface IitemRepository
    {
        Task<Item>getItemByIdAsync(int id);
         Task<IReadOnlyList<Item>> GetItemsAsync();
    }
}
