using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Models.DTOS;

namespace UserService.Data
{
    public class IdentityContext : IdentityDbContext<AppUser>
    {
        public List <RefreshTokens>  _refreshTokens;

        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }////****chainig constructor

    }
}
