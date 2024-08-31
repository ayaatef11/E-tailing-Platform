
namespace Core.Specifications.ProductSpecifications
{
    //attributes can start with small letters but fields must start with capital letters
    public class ProductSpecificationParameters
    {
        private const int MaxPageSize = 10;

        private int pageSize = 5;
        public int PageIndex { get; set; } = 1;
/// <summary>
/// so important 
/// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? pageSize : value; }
        }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }//filtering by category or brand
        public string? Sort { get; set; }
        public string? Search { get; set; }

    }
}