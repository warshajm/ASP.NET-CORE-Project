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

            // seed new categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = "Cat001", Name = "Espresso Beans" },
                new Category { Id = "Cat002", Name = "Dark Roast" },
                new Category { Id = "Cat003", Name = "Light Roast" },
                new Category { Id = "Cat004", Name = "Medium Roast" },
                new Category { Id = "Cat005", Name = "Decaf" }
            );

            // seed coffee products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Premium Espresso Beans",
                    Price = 14.99m,
                    Description = "Rich and bold premium espresso beans.",
                    Image = "espresso_roast.jpeg",
                    Stock = 100,
                    CategoryId = "Cat001"
                },
                new Product
                {
                    Id = 2,
                    Name = "French Dark Roast",
                    Price = 13.99m,
                    Description = "Intense and smoky dark roast coffee.",
                    Image = "dark_roast.jpeg",
                    Stock = 120,
                    CategoryId = "Cat002"
                },
                new Product
                {
                    Id = 3,
                    Name = "Light Roast Blend",
                    Price = 12.99m,
                    Description = "Bright and fruity light roast blend.",
                    Image = "light_roast.jpeg",
                    Stock = 150,
                    CategoryId = "Cat003"
                },
                new Product
                {
                    Id = 4,
                    Name = "Classic Medium Roast",
                    Price = 13.49m,
                    Description = "Well-balanced medium roast coffee.",
                    Image = "medium_roast.jpeg",
                    Stock = 130,
                    CategoryId = "Cat004"
                },
                new Product
                {
                    Id = 5,
                    Name = "Decaf Coffee",
                    Price = 11.99m,
                    Description = "Smooth and flavorful decaf coffee.",
                    Image = "decaf_coffee.jpeg",
                    Stock = 140,
                    CategoryId = "Cat005"
                }
            );
        }
    }
}
