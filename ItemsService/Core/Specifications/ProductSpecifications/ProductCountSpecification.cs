﻿using Core.Entities.ProductEntities;

namespace Core.Specifications.ProductSpecifications
{
    public class ProductCountSpecification : BaseSpecification<Product>
    {//filter items 
        //seach property allows the user to enter the search term 
        public ProductCountSpecification(ProductSpecificationParameters specParams)
        {
            WhereCriteria =
               p => (string.IsNullOrEmpty(specParams.Search) || p.Name.ToLower().Contains(specParams.Search.ToLower())) &&
               (!specParams.BrandId.HasValue || p.BrandId == specParams.BrandId.Value) &&
               (!specParams.CategoryId.HasValue || p.CategoryId == specParams.CategoryId.Value);
        }

    }
}