using Core.Entities;
using OrdersAndItemsService.Core.Models;

namespace OrdersAndItemsService.Core.interfaces.Repositories
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        Task<int> CompleteAsync();
    }
}