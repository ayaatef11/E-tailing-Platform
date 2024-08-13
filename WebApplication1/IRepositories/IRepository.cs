using System.Linq.Expressions;

namespace WebApplication1.Iservices
{
    public interface IRepository <T> where T : class
{
            T GetById(int Id);
           IReadOnlyList <T> GetAll();

            IQueryable<T> GetAllQueryable();

            IEnumerable<T> GetAll(Expression<Func<T, bool>> match);

            bool Save(T model);

            bool Update(T model);

            bool Delete(T model);

            bool Exist(Expression<Func<T, bool>> match);

            //Async
            Task<T> GetByIdAsync(int Id);

            Task<IEnumerable<T>> GetAllAsync();

            Task<bool> SaveAsync(T model);

            Task<bool> UpdateAsync(T model);

            Task<bool> DeleteAsync(T model);

            Task<bool> ExistAsync(Expression<Func<T, bool>> match);
            //Task<T> ExistItemAsync(Expression<Func<T, bool>> match);

            Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> match);

        }

    }

