namespace WebApplication1.Configuration
{
    public class jwtConfig
    {
        public string Secret { get; set; }=string.Empty;
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string ExpiryMinutes { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;

    }
}
