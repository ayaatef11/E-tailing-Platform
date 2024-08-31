
using Core.Entities.ProductEntities;
using Core.interfaces.Repositories;
using Core.interfaces.Services;
using Core.Specifications.ProductSpecifications;


namespace Service.Service
{
    public class ProductService(IUnitOfWork unitOfWork) : IProductService
    {
        public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecificationParameters specParams)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(specParams);
            var products = await unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
            return products;
        }
        public async Task<int> GetProductCount(ProductSpecificationParameters specParams)
        {
            var spec = new ProductCountSpecification(specParams);
            var productsCount = await unitOfWork.Repository<Product>().GetCountAsync(spec);
            return productsCount;
        }
        public async Task<Product?> GetProductAsync(int id)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(id);
            var product = await unitOfWork.Repository<Product>().GetByIdWithSpecAsync(spec);
            return product;
        }
        public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
        {
            var brands = await unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return brands;
        }
        public async Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync()
        {
            var categories = await unitOfWork.Repository<ProductCategory>().GetAllAsync();
            return categories;
        }
    }
}