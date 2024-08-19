using System.Collections.Generic;
using System.Threading.Tasks;

public interface IGenericRepository<T> where T : class
{
    // Retrieve an entity by its ID
    Task<T> GetByIdAsync(int id);

    // Retrieve all entities
    Task<IReadOnlyList<T>> GetAllAsync();

    // Add a new entity
    Task AddAsync(T entity);

    // Update an existing entity
    Task UpdateAsync(T entity);

    // Delete an entity by its ID
    Task DeleteAsync(int id);

    // Optionally: Save changes to the database (depends on your implementation)
    Task SaveChangesAsync();
}
