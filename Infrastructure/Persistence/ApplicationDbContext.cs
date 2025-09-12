using Dapper_StoredProcedures.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dapper_StoredProcedures.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .HasOne<Category>()
            .WithMany()
            .HasForeignKey(p => p.CategoryId);

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.SKU).IsUnique();

        modelBuilder.Entity<Product>()
          .Property(p => p.Price)
          .HasPrecision(18, 2);

        modelBuilder.Entity<Product>()
            .HasIndex(p => new { p.CategoryId, p.Name, }).IsUnique();



        modelBuilder.Entity<Order>()
            .HasOne<Customer>()
            .WithMany()
            .HasForeignKey(o => o.CustomerId);

        modelBuilder.Entity<Order>()
          .Property(o => o.TotalAmount)
          .HasPrecision(18, 2);

        modelBuilder.Entity<OrderItem>()
            .HasKey(oi => new { oi.OrderId, oi.ProductId });

        modelBuilder.Entity<OrderItem>()
            .HasOne<Order>()
            .WithMany()
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(oi => oi.ProductId);

        modelBuilder.Entity<Payment>()
            .HasKey(p => p.OrderId);
        modelBuilder.Entity<Payment>()
          .HasOne<Order>()
          .WithOne()
          .HasForeignKey<Payment>(p => p.OrderId);

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronics" },
            new Category { Id = 2, Name = "Books" },
            new Category { Id = 3, Name = "Clothing" },
            new Category { Id = 4, Name = "Toys" },
            new Category { Id = 5, Name = "Sports" }
        );

        modelBuilder.Entity<Product>().HasData(
           new Product { Id = 1, SKU = "LAP-001", Name = "Laptop", Price = 1000, Quantity = 10, CategoryId = 1 },
           new Product { Id = 2, SKU = "SMP-001", Name = "Smartphone", Price = 500, Quantity = 20, CategoryId = 1 },
           new Product { Id = 3, SKU = "BOK-001", Name = "Novel", Price = 20, Quantity = 50, CategoryId = 2 },
           new Product { Id = 4, SKU = "CLT-001", Name = "T-Shirt", Price = 15, Quantity = 100, CategoryId = 3 },
           new Product { Id = 5, SKU = "SPT-001", Name = "Basketball", Price = 30, Quantity = 25, CategoryId = 5 }
       );


        modelBuilder.Entity<Customer>().HasData(
            new Customer { Id = 1, Name = "Alice", Email = "alice@example.com" },
            new Customer { Id = 2, Name = "Bob", Email = "bob@example.com" },
            new Customer { Id = 3, Name = "Charlie", Email = "charlie@example.com" },
            new Customer { Id = 4, Name = "David", Email = "david@example.com" },
            new Customer { Id = 5, Name = "Eva", Email = "eva@example.com" }
        );

        modelBuilder.Entity<Order>().HasData(
             new Order { Id = 1, CustomerId = 1, OrderDate = new DateTime(2025, 9, 12, 10, 0, 0), TotalAmount = 1500 },
             new Order { Id = 2, CustomerId = 2, OrderDate = new DateTime(2025, 9, 13, 10, 0, 0), TotalAmount = 520 },
             new Order { Id = 3, CustomerId = 3, OrderDate = new DateTime(2025, 9, 14, 10, 0, 0), TotalAmount = 35 },
             new Order { Id = 4, CustomerId = 4, OrderDate = new DateTime(2025, 9, 15, 10, 0, 0), TotalAmount = 45 },
             new Order { Id = 5, CustomerId = 5, OrderDate = new DateTime(2025, 9, 16, 10, 0, 0), TotalAmount = 100 }
         );


        modelBuilder.Entity<OrderItem>().HasData(
            new OrderItem { OrderId = 1, ProductId = 1, Quantity = 1 },
            new OrderItem { OrderId = 1, ProductId = 2, Quantity = 1 },
            new OrderItem { OrderId = 2, ProductId = 2, Quantity = 1 },
            new OrderItem { OrderId = 3, ProductId = 3, Quantity = 1 },
            new OrderItem { OrderId = 4, ProductId = 4, Quantity = 3 },
            new OrderItem { OrderId = 5, ProductId = 5, Quantity = 2 }
        );
        modelBuilder.Entity<Payment>().HasData(
          new Payment { OrderId = 1, Method = "CreditCard", PaymentDate = new DateTime(2025, 9, 12), Status = "Completed" },
          new Payment { OrderId = 2, Method = "PayPal", PaymentDate = new DateTime(2025, 9, 13), Status = "Completed" },
          new Payment { OrderId = 3, Method = "Cash", PaymentDate = new DateTime(2025, 9, 14), Status = "Pending" },
          new Payment { OrderId = 4, Method = "CreditCard", PaymentDate = new DateTime(2025, 9, 15), Status = "Pending" },
          new Payment { OrderId = 5, Method = "BankTransfer", PaymentDate = new DateTime(2025, 9, 16), Status = "Completed" }
      );
    }
}
