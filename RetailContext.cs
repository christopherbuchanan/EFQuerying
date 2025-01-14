using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EFQuerying;

public class RetailContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<OrderDetail> OrderDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Replace with your actual connection string  
        optionsBuilder.UseSqlite("Data Source=retail.db", x => x.MigrationsAssembly("EFQuerying"))
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine, LogLevel.Information);
        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        optionsBuilder.UseSeeding((context, _) =>
        {
            context.Add(
                new Customer
                {
                    CustomerID = 1,
                    Name = "John Doe",
                    Email = "johndoe@email.com",
                    RegistrationDate = DateTime.Now.AddDays(-30)
                });
            context.Add(new Customer
            {
                CustomerID = 2,
                Name = "Jane Doe",
                Email = "jane@gmail.com",
                RegistrationDate = DateTime.Now.AddDays(-3)
            });

            context.Add(new Product
            {
                ProductID = 1,
                Name = "Laptop",
                Price = 1200,
                Category = "Electronics"
            });
            context.Add(new Product
            {
                ProductID = 2,
                Name = "Mouse",
                Price = 20,
                Category = "Accessories"
            });
            context.Add(new Product
            {
                ProductID = 3,
                Name = "Keyboard",
                Price = 50,
                Category = "Accessories"
            });
            context.Add(new Product
            {
                ProductID = 4,
                Name = "Monitor",
                Price = 300,
                Category = "Electronics"
            });

            context.Add(new Order
            {
                OrderID = 1,
                CustomerID = 1,
                OrderDate = DateTime.Now.AddDays(-10),
                TotalAmount = 1200
            });

            context.Add(new Order
            {
                OrderID = 2,
                CustomerID = 1,
                OrderDate = DateTime.Now.AddDays(-5),
                TotalAmount = 320
            });

            context.Add(new Order
            {
                OrderID = 3,
                CustomerID = 2,
                OrderDate = DateTime.Now.AddDays(-2),
                TotalAmount = 50
            });

            context.Add(new OrderDetail
            {
                OrderDetailID = 1,
                OrderID = 1,
                ProductID = 1,
                Quantity = 1,
                UnitPrice = 1200
            });

            context.Add(new OrderDetail
            {
                OrderDetailID = 2,
                OrderID = 2,
                ProductID = 3,
                Quantity = 10,
                UnitPrice = 1200
            });

            context.Add(new OrderDetail
            {
                OrderDetailID = 3,
                OrderID = 2,
                ProductID = 3,
                Quantity = 10,
                UnitPrice = 1200
            });

            context.Add(new OrderDetail
            {
                OrderDetailID = 4,
                OrderID = 2,
                ProductID = 2,
                Quantity = 1,
                UnitPrice = 20
            });

            context.SaveChanges();
        });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure primary keys  
        modelBuilder.Entity<Customer>().HasKey(c => c.CustomerID);
        modelBuilder.Entity<Order>().HasKey(o => o.OrderID);
        modelBuilder.Entity<Product>().HasKey(p => p.ProductID);
        modelBuilder.Entity<OrderDetail>().HasKey(od => od.OrderDetailID);

        // Configure relationships  

        // Customer has many Orders  
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerID);

        // Order has many OrderDetails  
        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.OrderID);

        // Product has many OrderDetails  
        modelBuilder.Entity<OrderDetail>()
            .HasOne(od => od.Product)
            .WithMany(p => p.OrderDetails)
            .HasForeignKey(od => od.ProductID);

        // Configure property constraints and data types  

        // For example, setting decimal precision  
        modelBuilder.Entity<Order>()
            .Property(o => o.TotalAmount)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<OrderDetail>()
            .Property(od => od.UnitPrice)
            .HasColumnType("decimal(18,2)");
    }
}
