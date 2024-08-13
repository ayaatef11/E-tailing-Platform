using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Data.Enums;

namespace WebApplication1.Models
{
    public class Order
    {
        public int Id { get; set; }
        [ForeignKey(nameof(AppUser))]
        public int userId { get; set; }
        AppUser? user { get; set; }
        public DateTime OrderDate { get; set; }
        orderStatus orderStatus { get; set; }
        public int TotalAmount { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public string BillingAddress { get; set; } = string.Empty;
        PaymentMethod paymentMethod { get; set; }
        public List<Item>? Items { get; set; }
    }
}
