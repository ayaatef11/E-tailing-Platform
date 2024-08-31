namespace Core.Entities.OrderEntities
{

    public class OrderAddress //no id it is weak entity
    {
        public OrderAddress()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Street = string.Empty;
            City = string.Empty;
            Country = string.Empty;
        }
        public OrderAddress(string firstName, string lastName, string street, string city, string country)
        {
            FirstName = firstName;

            LastName = lastName;
            Street = street;
            City = city;
            Country = country;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }

}
