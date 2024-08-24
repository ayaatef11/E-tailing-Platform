
using OrdersAndItemsService.Core.Entities;
using OrdersAndItemsService.Core.interfaces.Repositories;
using OrdersAndItemsService.Core.Models;
using OrdersAndItemsService.Repository.Repositories;
using System.Collections;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _storeContext;
        private Hashtable _repositories;

        public UnitOfWork(DbContext storeContext)
        {
            _storeContext = storeContext;
            _repositories = new Hashtable();
        }

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            var key = typeof(T).Name;

            if (!_repositories.ContainsKey(key))
            {
                var repositoryType = typeof(GenericRepository<>);

                var repository = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _storeContext);

                _repositories.Add(key, repository);
            }

            return (GenericRepository<T>)_repositories[key];
        }

        public async Task<int> CompleteAsync()
        {
            return await _storeContext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _storeContext.DisposeAsync();
        }
    }
}