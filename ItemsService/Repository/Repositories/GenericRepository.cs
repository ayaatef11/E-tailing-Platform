


using Core.interfaces.Repositories;
using Core.interfaces.Specifications;
using Repository.Data;
using Core.Entities;

namespace Repository.Repositories
{
    public class GenericRepository<T>(StoreContext storeContext) : IGenericRepository<T> where T : BaseEntity
    {
        public async Task<IReadOnlyList<T>> GetAllAsync() => await storeContext.Set<T>().ToListAsync();
        public async Task<T?> GetByIdAsync(int id) => await storeContext.Set<T>().FindAsync(id);
        ///
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
        /// <summary>
        /// 
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