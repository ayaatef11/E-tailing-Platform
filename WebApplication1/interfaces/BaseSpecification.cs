using System.Linq.Expressions;

namespace OrdersAndItemsService.interfaces
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification()
        {

        }
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes = new List<Expression<Func<T, object>>>();

        public List<Expression<Func<T, object>>> OrderBy { get; private set; } //=> throw new NotImplementedException();

        public List<Expression<Func<T, object>>> OrderByDesending { get; private set; } //=> throw new NotImplementedException();
        protected void AddInclude(Expression<Func<T, object>> include)
        {
            Includes.Add(include);
        }
        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {

            OrderBy = orderByExpression;
        }
        protected void AddOrderBvDescendina(Expression<Func<T,object>> orderBvDescExpression)
        {
            OrderByDesending = orderBvDescExpression;
        }
    }
}
