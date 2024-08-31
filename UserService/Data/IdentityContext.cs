
using Models;

namespace UserService.Data
{
    public class IdentityContext/*(DbContextOptions<IdentityContext> options) */: IdentityDbContext<AppUser>/*(options)*/
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }
        public DbSet<RefreshTokens> _refreshTokens { get; set; }
       // public DbSet<AppUser> _users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Address>().ToTable("Addresses");
            /* builder.Entity<AppUser>(entity =>
             {
                 entity.ToTable("AspNetUsers");
             });*/
            builder.Entity<RefreshTokens>()
       .HasOne(rt => rt.User)             // Configure one-to- one or one-to-many relationship
       .WithMany()                        // If the User can have multiple RefreshTokens
       .HasForeignKey(rt => rt.AppUserId)    // Set foreign key property
       .OnDelete(DeleteBehavior.Cascade); // Define delete behavior (e.g., delete tokens if user is deleted)


            var admin = new IdentityRole("Admin") { NormalizedName = "admin" };
            var client = new IdentityRole("client") { NormalizedName = "client" };


            var seller = new IdentityRole("seller") { NormalizedName = "seller" };
                    
            
            builder.Entity<IdentityRole>().HasData(admin, seller, client);
        }

        }
    }
