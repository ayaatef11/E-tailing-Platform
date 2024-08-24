

using OrdersAndItemsService.Core.Entities;

namespace OrdersAndItemsService.Core.interfaces.Repositories
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        Task<int> CompleteAsync();
    }
}