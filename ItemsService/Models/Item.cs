using Microsoft.AspNetCore.Http;
using OrdersAndItemsService.Models;
using OrdersAndItemsService.Models.OrderEntities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Numerics;

namespace WebApplication1.Models
{
    public class Item:BaseEntity
    {
        
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        Order? Order { get; set; }
        public string Name { get; set; }=string.Empty;
        public decimal Price { get; set; }
        public double Quantity { get; set; }
        public string Image { get; set; }=string.Empty;
    }
}
