using Core.Entities;
using OrdersAndItemsService.Core.Models;

namespace OrdersAndItemsService.Repository.IRepositories
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        Task<int> CompleteAsync();
    }
}