using System.Runtime.Serialization;

namespace Core.Entities.OrderEntities
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "Payment Succeeded")]
        PaymentSucceeded,
        [EnumMember(Value = "Payment Failed")]
        PaymentFailed
    }
}