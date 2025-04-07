using Assignment2.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment2.Data
{
    public class DealsFinderDbContext: DbContext
    {
        public DealsFinderDbContext(DbContextOptions<DealsFinderDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<FoodDeliveryService> FoodDeliveryServices { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Deal> Deals { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<FoodDeliveryService>().ToTable("FoodDeliveryService");
            modelBuilder.Entity<Subscription>().ToTable("Subscription");
            modelBuilder.Entity<Subscription>().HasKey(s => new { s.CustomerId, s.ServiceId });
            modelBuilder.Entity<Deal>().ToTable("Deal");
        }

      
    }
}
