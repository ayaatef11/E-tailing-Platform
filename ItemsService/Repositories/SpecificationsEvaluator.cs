using Core.Entities;
using Microsoft.EntityFrameworkCore;
using OrdersAndItemsService.interfaces;
using OrdersAndItemsService.Models;
using System.Linq;

namespace Repository
{
    public class SpecificationsEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecifications<T> spec)
        {
            var query = inputQuery;

            if (spec.WhereCriteria != null)
                query = query.Where(spec.WhereCriteria);

            if (spec.OrderBy != null)
                query = query.OrderBy(spec.OrderBy);
            else if (spec.OrderByDesc != null)
                query = query.OrderByDescending(spec.OrderByDesc);

            if (spec.IsPaginationEnabled)
                query = query.Skip(spec.Skip).Take(spec.Take);

            query = spec.IncludesCriteria.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            return query;
        }
    }
}