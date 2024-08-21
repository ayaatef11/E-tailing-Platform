using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OrdersAndItemsService.Core.interfaces;

public class SpecificationEvaluator<T> where T : class
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecifications<T> specification)
    {
        var query = inputQuery;

        // Apply criteria filtering
        if (specification.Criteria != null)
        {
            query = query.Where(specification.Criteria);
        }

        // Apply eager loading (Includes)
        query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

        // Apply ordering
        if (specification.OrderBy != null)
        {
            query = specification.OrderBy(query);
        }
        else if (specification.OrderByDescending != null)
        {
            query = specification.OrderByDescending(query);
        }

        return query;
    }
}
