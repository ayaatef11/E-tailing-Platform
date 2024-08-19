using Core.Entities.Basket_Entities;

namespace OrdersAndItemsService.Models
{
    public class CustomerBasket
    {
        public CustomerBasket()
         {

         }
        public CustomerBasket(string Id)
        {
            this.Id = Id;
        }
        public string Id { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
        public int DeliveryMethodId { get; set; }
        public string ClientSecret { get; set; }=string.Empty;
        public string PaymnetIntentId {  get; set; }= string.Empty;
    }
}
