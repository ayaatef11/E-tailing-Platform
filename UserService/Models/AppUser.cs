using Microsoft.AspNetCore.Identity;

namespace Models
{
    public class AppUser:IdentityUser
    {   
        public int Id { get; set; }
        public string DisplayName { get; set; }=string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public Address Address { get; set; }


    }
}
