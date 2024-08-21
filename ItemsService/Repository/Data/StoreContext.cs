using Microsoft.EntityFrameworkCore;
using OrdersAndItemsService.Core.Models.BasketEntites;
using OrdersAndItemsService.Core.Models.OrderEntities;
using OrdersAndItemsService.Models;
using Stripe;

namespace OrdersAndItemsService.Repository.Data
{
    public class StoreContext : DbContext
    {
        // Constructor to initialize the DbContext with options
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        // DbSets represent tables in the database
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Customer> Customers { get; set; }

        // Configure model relationships and constraints (optional)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
