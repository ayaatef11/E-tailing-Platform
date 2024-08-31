

using Core.Entities;

namespace Core.Entities.OrderEntities
{

    public class OrderDeliveryMethod : BaseEntity
    {

        public OrderDeliveryMethod()
        {
            Name = string.Empty;
            Description = string.Empty;
            Cost = 0;
            DeliveryTime = string.Empty;
        }
        public OrderDeliveryMethod(string name, string description, decimal cost, string deliveryTime)
        {
            Name = name;
            Description = description;
            Cost = cost;
            DeliveryTime = deliveryTime;
        }
        public string Name { get; set; } 
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public string DeliveryTime { get; set; }
    }
}