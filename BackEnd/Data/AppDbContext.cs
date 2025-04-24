using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShoppingAppDB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingAppDB.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public IQueryable<CartDetails> GetCartItems(int cartId)
        {
            return this.Set<CartDetails>().FromSqlInterpolated($"select * from GetCartItems({cartId})");
        }

        public IQueryable<OrderDetails> GetOrderItems(int orderId)
        {
            return this.Set<OrderDetails>().FromSqlInterpolated($"select * from GetOrderItems({orderId})");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(config.GetSection("constr").Value)
                .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            modelBuilder.Entity<CartDetails>()
                .ToFunction("GetCartItems")
                .HasNoKey();

            modelBuilder.Entity<OrderDetails>()
                .ToFunction("GetOrderItems")
                .HasNoKey();

            modelBuilder.Entity<OrderDetails>(entity =>
            {
                entity.Property(e => e.UnitPrice)
                    .HasColumnType("decimal(18, 2)"); 
            });
        }
    }
}
