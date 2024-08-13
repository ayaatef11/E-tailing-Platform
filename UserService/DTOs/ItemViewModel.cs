using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Numerics;

namespace WebApplication1.Models
{
    public class Item
    {

        public int Id { get; set; }
        [ForeignKey(nameof(Order))]
        public int orderId { get; set; }
        Order? order { get; set; }
        public string Name { get; set; } = string.Empty;
        public double price { get; set; }
        public double quantity { get; set; }
        public string Image { get; set; } = string.Empty;
    }
}
