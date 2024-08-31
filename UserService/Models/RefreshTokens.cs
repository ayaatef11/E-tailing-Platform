


namespace Models
{
    public class RefreshTokens
    {
        public int Id { get; set; }
        public string AppUserId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string JwtId { get; set; } = string.Empty;
        public bool IsUsed { get; set; }
        public bool IsRevorked { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        [ForeignKey(nameof(AppUserId))]
        public AppUser User { get; set; } = null!;
    }
}
