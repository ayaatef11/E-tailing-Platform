

using OrdersAndItemsService.Core.Models.OrderEntities;

namespace OrdersAndItemsService.Core.Entities.OrderEntities
{
    public class Order : BaseEntity
    {
        public Order()
        {
            // we create this constractor because EF need it while migration
        }
        public Order(string buyerEmail, OrderAddress shippingAddress, OrderDeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
        }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.pending;                                                   // so we create configuration for it
        public OrderAddress ShippingAddress { get; set; }
        public OrderDeliveryMethod? DeliveryMethod { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
        public decimal SubTotal { get; set; }
    
        public decimal GetTotal() => SubTotal + DeliveryMethod.Cost;//this is a dervided attribute we can make it also by the readonly property

        public string PaymentIntentId { get; set; } = string.Empty;
    }
}