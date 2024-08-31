namespace Helpers
{
    public class JWT
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; }=string.Empty;
        public string DurationInDays { get; set; } = string.Empty;
    }
}
