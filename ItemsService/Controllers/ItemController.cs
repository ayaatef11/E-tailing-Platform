using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrdersAndItemsService.Controllers;
using OrdersAndItemsService.Core.Models;
using OrdersAndItemsService.Errors;

namespace API.Controllers
{

    public class ProductController(IProductService productService, IMapper mapper) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecificationParameters specParams)
        {
            var products = await productService.GetProductsAsync(specParams);

            var productsDto = mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            var productsCount = await productService.GetProductCount(specParams);

            return Ok(new PaginationToReturn<ProductToReturnDto>(specParams.PageIndex, specParams.PageSize, productsCount, productsDto));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProduct(int id)
        {
            var product = await productService.GetProductAsync(id);

            if (product is null)
                return NotFound(new ApiResponse(404));

            return Ok(mapper.Map<Product, ProductToReturnDto>(product));
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await productService.GetBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetCategories()
        {
            var categories = await productService.GetCategoriesAsync();
            return Ok(categories);
        }
    }
}
