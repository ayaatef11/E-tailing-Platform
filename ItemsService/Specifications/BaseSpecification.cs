using Core.Entities;
using OrdersAndItemsService.interfaces;
using OrdersAndItemsService.Models;
using System.Linq.Expressions;

namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecifications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> WhereCriteria { get; set; }
        public List<Expression<Func<T, object>>> IncludesCriteria { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }

        public BaseSpecification()
        {
            // -- Here WhereCriteria is null
        }

        public BaseSpecification(Expression<Func<T, bool>> whereCriteria)
        {
            WhereCriteria = whereCriteria;
        }

        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }

        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes{get;}

        List<Expression<Func<T, object>>> ISpecifications<T>.OrderBy => throw new NotImplementedException();

        public List<Expression<Func<T, object>>> OrderByDesending => throw new NotImplementedException();

        public void ApplyPagination(int skip, int take)
        {
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;
        }
    }
}