namespace UserService.Models
{
    public class AccessToken
   {
            public string Token { get; set; }
            public DateTime Expiry { get; set; }
            public string TokenType { get; set; }
            public string Scope { get; set; }
            public bool IsExpired() => DateTime.UtcNow >= Expiry;
        }

    }

