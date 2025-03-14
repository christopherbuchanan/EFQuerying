using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EFQuerying;

public class RetailContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }

    public DbSet<Order> Orders { get; set; }
    public DbSet<ExpeditedOrder> ExpeditedOrders { get; set; }
    public DbSet<NextDayOrder> NextDayOrders { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<OrderDetail> OrderDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Replace with your actual connection string  
        //optionsBuilder.UseSqlite("Data Source=retail.db", x => x.MigrationsAssembly("EFQuerying"))
        //    .EnableSensitiveDataLogging()
        //    .LogTo(Console.WriteLine, LogLevel.Information);

        optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EFQuerying", x => x.MigrationsAssembly("EFQuerying"))
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine, LogLevel.Information);

        optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        optionsBuilder.UseSeeding((context, _) =>
        {
            context.Add(SeedCustomers.JohnDoe);
            context.Add(SeedCustomers.JaneDoe);

            context.Add(new Product
            {
                Name = "Laptop",
                Price = 1200,
                Category = "Electronics"
            });
            var mouse = new Product
            {
                Name = "Mouse",
                Price = 20,
                Category = "Accessories"
            };
            var keyboard = new Product
            {
                Name = "Keyboard",
                Price = 50,
                Category = "Accessories"
            };
            var monitor = new Product
            {
                Name = "Monitor",
                Price = 300,
                Category = "Electronics"
            };

            var order1 = new Order
            {
                Customer = SeedCustomers.JohnDoe,
                OrderDate = DateTime.Now.AddDays(-10),
                TotalAmount = 1200
            };
            var order2 = new Order
            {
                Customer = SeedCustomers.JohnDoe,
                OrderDate = DateTime.Now.AddDays(-5),
                TotalAmount = 320
            };
            var order3 = new Order
            {
                Customer = SeedCustomers.JaneDoe,
                OrderDate = DateTime.Now.AddDays(-2),
                TotalAmount = 50
            };
            context.Add(order1);
            context.Add(order2);
            context.Add(order3);

            context.Add(new OrderDetail
            {
                Order = order1,
                Product = mouse,
                Quantity = 1,
                UnitPrice = 1200
            });

            context.Add(new OrderDetail
            {
                Order = order2,
                Product = monitor,
                Quantity = 10,
                UnitPrice = 1200
            });

            context.Add(new OrderDetail
            {
                Order = order2,
                Product = keyboard,
                Quantity = 10,
                UnitPrice = 1200
            });

            context.Add(new OrderDetail
            {
                Order = order3,
                Product = mouse,
                Quantity = 1,
                UnitPrice = 20
            });

            context.Add(new ExpeditedOrder
            {
                Customer = SeedCustomers.JaneDoe,
                OrderDate = DateTime.Now.AddDays(-2),
                TotalAmount = 50,
                DaysToDeliver = 2,
                ShippingMethod = "Post Office"
            });

            context.Add(new NextDayOrder
            {
                Customer = SeedCustomers.JaneDoe,
                OrderDate = DateTime.Now,
                TotalAmount = 500,
                ShippingMethod = "Super Fast Courier",
                AdditionalCharge = 10
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

public static class SeedCustomers
{
    public static Customer JaneDoe => new()
    {
        Name = "Jane Doe",
        Email = ""
    };
    public static Customer JohnDoe => new()
    {
        Name = "John Doe",
        Email = "johndoe@email.com",
        RegistrationDate = DateTime.Now.AddDays(-30)
    };
}