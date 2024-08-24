using System.Linq.Expressions;

namespace OrdersAndItemsService.Core.interfaces.Specifications
{
    public interface ISpecifications<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<Expression<Func<T, object>>> OrderBy { get; }
        List<Expression<Func<T, object>>> OrderByDesending { get; }
    }
}
