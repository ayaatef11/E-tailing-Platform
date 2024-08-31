using Core.Entities.OrderEntities;
using Core.Entities.ProductEntities;


namespace Repository.Data
{
    public class StoreContext(DbContextOptions<StoreContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderDeliveryMethod> OrderDeliveryMethods { get; set; }
        public DbSet<ProductOrderItem> ProductOrderItem { get; set; }
    }
}