using Core.Entities.ProductEntities;

namespace Core.Specifications.ProductSpecifications
{
    public class ProductCountSpecification : BaseSpecification<Product>
    {//filter items 
        //seach property allows the user to enter the search term 
        public ProductCountSpecification(ProductSpecificationParameters specParams)
        {
            WhereCriteria =
               p => (string.IsNullOrEmpty(specParams.search) || p.Name.ToLower().Contains(specParams.search.ToLower())) &&
               (!specParams.brandId.HasValue || p.BrandId == specParams.brandId.Value) &&
               (!specParams.categoryId.HasValue || p.CategoryId == specParams.categoryId.Value);
        }

    }
}