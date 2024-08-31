
using Core.Entities;
using System.Linq.Expressions;

namespace Core.interfaces.Specifications;

public class/*interface*/ ISpecifications<T> where T : BaseEntity
{
    public ISpecifications()
    {
        WhereCriteria = null!;
        IncludesCriteria = [];
        OrderBy = null!;
        OrderByDesc = null!;
        Skip = 0;
        Take = 0;
        IsPaginationEnabled = false;
    }
    public Expression<Func<T, bool>> WhereCriteria { get; set; }
    public List<Expression<Func<T, object>>> IncludesCriteria { get; set; }
    public Expression<Func<T, object>> OrderBy { get; set; }
    public Expression<Func<T, object>> OrderByDesc { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; }
    public bool IsPaginationEnabled { get; set; }
}
