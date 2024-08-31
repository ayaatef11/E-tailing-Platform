using Core.Entities;

namespace Core.Entities.ProductEntities
{
    public class Product : BaseEntity
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public int BrandId { get; set; }
        public ProductBrand Brand { get; set; }
        public int CategoryId { get; set; }
        public ProductCategory Category { get; set; }
    }
}