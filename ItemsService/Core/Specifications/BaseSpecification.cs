﻿
using Core.Entities;
using Core.interfaces.Specifications;
using System.Linq.Expressions;

namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecifications<T> where T : BaseEntity
    {
        public BaseSpecification()
        {

        }

        public BaseSpecification(Expression<Func<T, bool>> whereCriteria)
        {
            WhereCriteria = whereCriteria;
        }

     /*   public Expression<Func<T, bool>> WhereCriteria { get; set; }
        public List<Expression<Func<T, object>>> IncludesCriteria { get; set; } = [];
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }*/


        public void ApplyPagination(int skip, int take)
        {
           IsPaginationEnabled = true;
            Skip = skip;
            Take = take;
        }
    }
}