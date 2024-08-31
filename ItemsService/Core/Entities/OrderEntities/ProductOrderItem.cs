namespace Core.Entities.OrderEntities
{
    public class ProductOrderItem
    {

        public ProductOrderItem()
        {
        ProductId = 0;
        ProductName =string.Empty;
        PictureUrl=string.Empty;
    }

        public ProductOrderItem(int productId, string productName, string pictureUrl)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
        }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
    }
}