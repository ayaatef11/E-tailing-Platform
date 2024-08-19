using Core.Entities.Basket_Entities;
using Core.Entities.Order_Entities;
using Microsoft.EntityFrameworkCore;
using OrdersAndItemsService.Models;
using Stripe;

namespace OrdersAndItemsService.Data
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
