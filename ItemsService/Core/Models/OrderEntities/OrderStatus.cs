using System.Runtime.Serialization;

namespace OrdersAndItemsService.Core.Models.OrderEntities
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        pending,
        [EnumMember(Value = "Payment Succeeded")]
        paymentSucceeded,
        [EnumMember(Value = "Payment Failed")]
        paymentFailed
    }
}