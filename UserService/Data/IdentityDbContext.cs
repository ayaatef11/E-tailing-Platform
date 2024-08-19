using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserService.Models;
using WebApplication1.Models;

namespace UserService.Data
{
    public class IdentityContext : IdentityDbContext<AppUser>
    {
        public List <RefreshTokens>  _refreshTokens;

        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }

    }
}
