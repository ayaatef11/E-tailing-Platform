using Core.Entities;
using OrdersAndItemsService.IRepositories;
using OrdersAndItemsService.Models;

namespace Core.Interfaces.Repositories
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        Task<int> CompleteAsync();
    }
}