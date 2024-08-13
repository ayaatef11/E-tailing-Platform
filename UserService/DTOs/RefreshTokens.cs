using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.DTOS
{
    public class RefreshTokens
    {
        public int Id { get; set; }
        public string userId { get; set; } = string.Empty;
        public string token { get; set; }=string.Empty;
        public string jwtId { get; set; } = string.Empty;
        public bool isUsed { get; set; } 
        public bool isRevorked { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        [ForeignKey(nameof(userId))]
        public IdentityUser User { get; set; }
    }
}
