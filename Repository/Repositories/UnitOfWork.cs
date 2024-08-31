
using Core.Entities;
using Core.interfaces.Repositories;
using System.Collections;


namespace Repository.Repositories
{
    public class UnitOfWork(DbContext storeContext) : IUnitOfWork
    {
        private readonly DbContext _storeContext = storeContext;
        private readonly Hashtable _repositories = [];
        private bool _disposed = false;
       
        public IGenericRepository<T> Repository<T>() where T : BaseEntity//generic function
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

        public async Task<int> CompleteAsync() => await _storeContext.SaveChangesAsync();


        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore();
            GC.SuppressFinalize(this);
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_storeContext != null)
            {
                await _storeContext.DisposeAsync();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _storeContext?.Dispose();//if not null then dispose 
                }

                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }
    }
}