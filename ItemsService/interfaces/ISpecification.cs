using System.Linq.Expressions;

namespace OrdersAndItemsService.interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T,bool>>Criteria { get; }
        List<Expression<Func<T,object>>>Includes { get; }
        List<Expression<Func<T,object>>>OrderBy { get; }
        List<Expression<Func<T,object>>>OrderByDesending { get; }
    }
}
