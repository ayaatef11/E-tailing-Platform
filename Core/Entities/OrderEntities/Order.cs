

using Core.Entities;

namespace Core.Entities.OrderEntities
{
    public class Order : BaseEntity
    {

        public Order()
        {

        }
        public Order(string buyerEmail, OrderAddress shippingAddress, OrderDeliveryMethod? deliveryMethod, ICollection<OrderItem> items, decimal subTotal)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items ?? new HashSet<OrderItem>();
            SubTotal = subTotal;
        }

        public string BuyerEmail { get; init; }
        public DateTimeOffset OrderDate { get; init; } = DateTimeOffset.UtcNow;
        public OrderStatus Status { get; init; } = OrderStatus.Pending;
        public OrderAddress ShippingAddress { get; init; }
        public OrderDeliveryMethod? DeliveryMethod { get; init; }
        public ICollection<OrderItem> Items { get; init; } //= new HashSet<OrderItem>();
        public decimal SubTotal { get; init; }

        public decimal Total => SubTotal + (DeliveryMethod?.Cost ?? 0);
        public string PaymentIntentId { get; set; } = string.Empty;
    }
}


