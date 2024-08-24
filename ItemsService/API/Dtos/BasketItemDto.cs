

namespace API.Dtos
{
    public class BasketItemDto
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public string PictureUrl { get; set; }

        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "invalid price!")]
        public decimal Price { get; set; }

        public string Category { get; set; }

        public string Brand { get; set; }

        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "invalid quantity!")]
        public int Quantity { get; set; }
    }
}