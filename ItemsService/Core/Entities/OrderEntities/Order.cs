﻿

namespace OrdersAndItemsService.Core.Models.OrderEntities
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
        //reference 
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.pending; // In this property
                                                                       // we need store in database string not number
                                                                       //navigation property                                                   // so we create configuration for it
        public OrderAddress ShippingAddress { get; set; } // this is a navigation property
                                                          // so EF will mapped it to database
                                                          // but we don't need that
                                                          // we need take his properties and mapped it in Order table
                                                          // so will make configration for that :)
        public OrderDeliveryMethod? DeliveryMethod { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>(); // -- HashSet to be list unique
        public decimal SubTotal { get; set; } // all salaries of items

        // -- here we put Total property but we need that: this property not mapped in database
        // -- because this is derived attribute, because we can bring his value from another attributes (Subtotal + delivery method cost)
        // -- so to make derived attribute we have two ways
        // -- first: with read only property and data annotation [NotMapped]
        // -- second: with function

        // -- first
        // [NotMapped]
        // public decimal Total => Subtotal + DeliveryMethod.Cost;
        // -- second
        public decimal GetTotal() => SubTotal + DeliveryMethod.Cost;

        public string PaymentIntentId { get; set; } = string.Empty;
    }
}