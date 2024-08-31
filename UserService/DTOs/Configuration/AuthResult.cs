namespace UserService.DTOs.Configuration
{
    public class AuthResult
    {
        public string Token { get; set; } = string.Empty;
        public List<string>? Errors { get; set; }
        public bool Success { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
    }
}
