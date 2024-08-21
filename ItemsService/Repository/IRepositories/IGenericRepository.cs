using Core.Entities;
using OrdersAndItemsService.Core.interfaces;
using OrdersAndItemsService.Core.Models;

namespace OrdersAndItemsService.Repository.IRepositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);
        Task<int> GetCountAsync(ISpecifications<T> spec);
        Task<T?> GetByIdWithSpecAsync(ISpecifications<T> spec);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}