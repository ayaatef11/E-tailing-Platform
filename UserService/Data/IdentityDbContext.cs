using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace UserService.Data
{
    public class IdentityContext : IdentityDbContext<AppUser>
    {
        public List   _refreshTokens;

        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }////****chainig constructor

    }
}
