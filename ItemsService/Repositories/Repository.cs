using System.Linq.Expressions;
using WebApplication1.Iservices;
using WebApplication1.Data;
using Microsoft.EntityFrameworkCore;
namespace WebApplication1.Services
{
    //use IQuerable
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public bool Delete(T model)
        {
            try
            {
                _context.Set<T>().Remove(model);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(T model)
        {
            try
            {
                _context.Set<T>().Remove(model);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        //---------------------
        public bool Exist(Expression<Func<T, bool>> match)
        {
            try
            {
                var Result = _context.Set<T>().FirstOrDefault(match);
                if (Result is not null)
                    return true;
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> ExistAsync(Expression<Func<T, bool>> match)
        {
            try
            {
                var Result = await _context.Set<T>().FirstOrDefaultAsync(match);
                if (Result is not null)
                    return true;
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //--------------------------
        public IReadOnlyList<T> GetAll()
        {
            try
            {
                return _context.Set<T>().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> match)
        {
            try
            {
                return _context.Set<T>().Where(match).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _context.Set<T>().ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> match)
        {
            try
            {
                return await _context.Set<T>().Where(match).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<T> GetAllQueryable()
        {
            try
            {
                IQueryable<T> query = _context.Set<T>();
                return query;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //----------------------
        public T GetById(int Id)
        {
            try
            {
                return _context.Set<T>().Find(Id)!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> GetByIdAsync(int Id)
        {
            try
            {
                return _context.Set<T>().FindAsync(Id).Result!;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //--------------
        public bool Save(T model)
        {
            try
            {
                if (model is not null)
                {
                    _context.Set<T>().Add(model);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> SaveAsync(T model)
        {
            try
            {
                if (model is not null)
                {
                    _context.Set<T>().Add(model);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        //---------------

        public bool Update(T model)
        {
            try
            {
                if (model is not null)
                {
                    _context.Set<T>().Update(model);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateAsync(T model)
        {
            try
            {
                if (model is not null)
                {
                    _context.Set<T>().Update(model);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
