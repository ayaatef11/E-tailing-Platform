using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stripe;
using UserService.Models;
using WebApplication1.Models;

namespace UserService.Data
{
    public class IdentityContext : IdentityDbContext<AppUser>
    {
        public DbSet<RefreshTokens> RefreshTokens { get; set; }

        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }
        // In this method we override OnModelCreating which exist in base class
        // so we need to call it
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Address>().ToTable("Addresses");
        }


        }
    }
