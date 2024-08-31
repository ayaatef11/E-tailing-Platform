

namespace Core.Entities.BasketEntites
{
    public class BasketItem
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string PictureUrl { get; set; } = string.Empty;

        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "Price number is illegal!")]
        public decimal Price { get; set; }

        public string Category { get; set; } = string.Empty;

        public string Brand { get; set; }=string.Empty; 

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity number is illegal!")]
        public int Quantity { get; set; }
    }
}