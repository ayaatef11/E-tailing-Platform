using Microsoft.AspNetCore.Identity;

namespace Models
{
    public class AppUser:IdentityUser
    {   
        //public int Id { get; set; }//The exception 'A key cannot be configured on 'AppUser' because it is a derived type. The key must be configured on the root type 'IdentityUser'
        public string DisplayName { get; set; }=string.Empty;
        public Address Address { get; set; }


    }
}
