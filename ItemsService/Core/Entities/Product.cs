

namespace OrdersAndItemsService.Core.Entities
{
    public class Product : BaseEntity
    {
        //it shouldn't contain the order id 
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public int BrandId { get; set; }//navigation for product brand and we make it in fluent api 
        public ProductBrand Brand { get; set; }
        public int CategoryId { get; set; }  
        public ProductCategory Category { get; set; } 
    }
}