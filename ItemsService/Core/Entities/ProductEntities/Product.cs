using Core.Entities;

namespace Core.Entities.ProductEntities
{
    public class Product : BaseEntity
    {

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;                                 
        public decimal Price { get; set; }
        public string PictureUrl { get; set; } = string.Empty;  
        public int BrandId { get; set; }
        public ProductBrand Brand { get; set; }
        public int CategoryId { get; set; }
        public ProductCategory Category { get; set; }
    }
}