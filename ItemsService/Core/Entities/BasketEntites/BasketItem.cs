﻿

namespace OrdersAndItemsService.Core.Entities.BasketEntites
{
    public class BasketItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PictureUrl { get; set; }

        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Price must be greater than zero!")]
        public decimal Price { get; set; }

        public string Category { get; set; }

        public string Brand { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least one item!")]
        public int Quantity { get; set; }
    }
}