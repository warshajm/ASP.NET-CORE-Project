using eCommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                    .HasKey(c => c.CustomerId);

            modelBuilder.Entity<Customer>()
                    .HasMany(c => c.Carts)
                    .WithOne(cart => cart.Customer)
                    .HasForeignKey(cart => cart.CustomerId);

            modelBuilder.Entity<Customer>()
                    .HasMany(c => c.Orders)
                    .WithOne(order => order.Customer)
                    .HasForeignKey(order => order.CustomerId);

            modelBuilder.Entity<Category>()
                    .HasMany(c => c.Products)
                    .WithOne(p => p.Category)
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);

            // Cart to Customer
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Customer)
                .WithMany()
                .HasForeignKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // CartItem to Cart
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // CartItem to Product
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order to Customer
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany()
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // OrderItem to Order
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // OrderItem to Product
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Product to Category
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // seed coffee categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = "Cat001", Name = "Espresso" },
                new Category { Id = "Cat002", Name = "Cold Brew" },
                new Category { Id = "Cat003", Name = "French Press" }
            );

            // seed coffee products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Espresso Beans",
                    Price = 12.99m,
                    Description = "Rich and bold espresso beans, perfect for a strong shot of espresso.",
                    Image = "espresso_beans.jpg",
                    Stock = 100,
                    CategoryId = "Cat001"
                },
                new Product
                {
                    Id = 2,
                    Name = "Double Espresso",
                    Price = 2.99m,
                    Description = "A double shot of strong and rich espresso.",
                    Image = "double_espresso.jpg",
                    Stock = 150,
                    CategoryId = "Cat001"
                },

                new Product
                {
                    Id = 3,
                    Name = "Cold Brew Coffee",
                    Price = 4.99m,
                    Description = "Smooth and refreshing cold brew coffee.",
                    Image = "cold_brew.jpg",
                    Stock = 75,
                    CategoryId = "Cat002"
                },
                new Product
                {
                    Id = 4,
                    Name = "Nitro Cold Brew",
                    Price = 5.99m,
                    Description = "Cold brew coffee infused with nitrogen for a creamy texture.",
                    Image = "nitro_cold_brew.jpg",
                    Stock = 60,
                    CategoryId = "Cat002"
                },

                new Product
                {
                    Id = 5,
                    Name = "French Press Coffee",
                    Price = 14.99m,
                    Description = "Coarsely ground coffee beans, ideal for French press brewing.",
                    Image = "french_press_coffee.jpg",
                    Stock = 80,
                    CategoryId = "Cat003"
                },
                new Product
                {
                    Id = 6,
                    Name = "Organic French Press Coffee",
                    Price = 16.99m,
                    Description = "Organic, fair-trade coffee beans for French press brewing.",
                    Image = "organic_french_press_coffee.jpg",
                    Stock = 70,
                    CategoryId = "Cat003"
                }
            );
        }
    }
}
