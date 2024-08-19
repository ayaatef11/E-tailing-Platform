
using System.Reflection;
using OrdersAndItemsService.Models.OrderEntities;
namespace WebApplication1.Data
{
    public class AppDbContext:DbContext

    {
        public object _refreshTokens;

        public AppDbContext(DbContextOptions options):base(options) 
        {
            
        }
       public virtual  DbSet<Item>Items { get; set; }
       public virtual DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
