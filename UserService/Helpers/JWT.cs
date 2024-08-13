namespace WebApplication1.Helpers
{
    public class JWT
    {
        public string key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; }=string.Empty;
        public string DurationInDays { get; set; } = string.Empty;
    }
}
