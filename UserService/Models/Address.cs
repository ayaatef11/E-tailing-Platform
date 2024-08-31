namespace Models
{
    public class Address
    {
        public int Id { get; set; }
        public string FirtName { get; set; } = string.Empty;
        public string LastName { get; set; }=string.Empty;
        public string Street { get; set; } = string.Empty;
        public string City { get; set; }=string.Empty ;
        public string Country { get; set; } = string.Empty;
        public string AppUserId { get; set; } = string.Empty;
   
    }
}