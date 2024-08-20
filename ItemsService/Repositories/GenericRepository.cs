using Core.Entities;
using Microsoft.EntityFrameworkCore;
using OrdersAndItemsService.Data;
using OrdersAndItemsService.interfaces;
using OrdersAndItemsService.Models;
using OrdersAndItemsService.IRepositories;
using System.Linq.Expressions;

namespace Repository
{
    public class GenericRepository<T>(StoreContext storeContext) : IGenericRepository<T> where T : BaseEntity
    {
        public async Task<IReadOnlyList<T>> GetAllAsync() => await storeContext.Set<T>().ToListAsync();
        public async Task<T?> GetByIdAsync(int id) => await storeContext.Set<T>().FindAsync(id);
        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
        {
            return await SpecificationsEvaluator<T>.GetQuery(storeContext.Set<T>(), spec).ToListAsync();
        }
        public async Task<int> GetCountAsync(ISpecifications<T> spec)
        {
            return await SpecificationsEvaluator<T>.GetQuery(storeContext.Set<T>(), spec).CountAsync();
        }
        public async Task<T?> GetByIdWithSpecAsync(ISpecifications<T> spec)
        {
            return await SpecificationsEvaluator<T>.GetQuery(storeContext.Set<T>(), spec).FirstOrDefaultAsync();
        }
        public async Task AddAsync(T entity)
        {
            await storeContext.Set<T>().AddAsync(entity);
        }

        public void Update(T entity)
        {
            storeContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            storeContext.Set<T>().Remove(entity);
        }

    }
}