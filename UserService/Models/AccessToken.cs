namespace Models;

    public class AccessToken
   {
            public string Token { get; set; }=string.Empty;
            public DateTime Expiry { get; set; }
            public string TokenType { get; set; } = string.Empty;
            public string Scope { get; set; } = string.Empty;
            public bool IsExpired() => DateTime.UtcNow >= Expiry;
        }

    

