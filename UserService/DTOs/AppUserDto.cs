using UserService.DTOs.Configuration;

namespace UserService.DTOs
{
    public class AppUserDto
    {
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; }=string.Empty;
        public AuthResult? JwtToken { get; set; }
    }
}