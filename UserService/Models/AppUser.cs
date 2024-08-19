using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class AppUser:IdentityUser
    {   
        public int Id { get; set; }
        public string Name { get; set; }=string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<Order>?orders { get; set; }


    }
}
