

namespace OrdersAndItemsService.Core.Models
{
    public class Product : BaseEntity
    {
        //it shouldn't contain the order id 
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public int BrandId { get; set; } // FK - ProductBrand - But We Don't Named ProductBrandId So EF Don't Know This FK So We Make It In Fluent API
        public ProductBrand Brand { get; set; } // Navigational Property
        public int CategoryId { get; set; } // FK - ProductCategory - But We Don't Named ProductCategoryId So EF Don't Know This FK So We Make It In Fluent API
        public ProductCategory Category { get; set; } // Navigational Property
    }
}