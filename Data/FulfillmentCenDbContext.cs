using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Data;


//TODO review: use standart EF pattern for DbContext: public FulfillmentCenDbContext(DbContextOptions<FulfillmentCenDbContext> options) : base(options) { }
//Without this, the context can't be properly registered with AddDbContext<>() in Program.cs, and the connection string can't be injected from configuration
//Since the full word is used everywhere else, this should be FulfillmentCenterDbContext for consistency
public class FulfillmentCenDbContext : DbContext
{
    public FulfillmentCenDbContext(DbContextOptions<FulfillmentCenDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<DistributionCenter> DistributionCenters { get; set; }
    public DbSet<Inventory> Inventories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<Customer> Customers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                Name = "Product A",
                SKU = "SKU-001",
                Weight = 1.5m
            },
            new Product
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                Name = "Product B",
                SKU = "SKU-002",
                Weight = 2.0m
            }
        );
        modelBuilder.Entity<DistributionCenter>().HasData(
            new DistributionCenter
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Central Hub",
                City = "Prague"
            },
            new DistributionCenter
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "North Depot",
                City = "Brno"
            },
            new DistributionCenter
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "South Warehouse",
                City = "Bratislava"
            },
            new DistributionCenter
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Name = "East Logistics Center",
                City = "Vienna"
            },
            new DistributionCenter
            {
                Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                Name = "West Distribution Point",
                City = "Budapest"
            }
        );
        
        modelBuilder.Entity<Inventory>().HasData(
            new Inventory
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                ProductId = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                DistributionCenterId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Quantity = 50
            },
            new Inventory
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                ProductId = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                DistributionCenterId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Quantity = 120
            },
            new Inventory
            {
                Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                ProductId = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                DistributionCenterId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Quantity = 30
            },
            new Inventory
            {
                Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                ProductId = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                DistributionCenterId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Quantity = 75
            },
            new Inventory
            {
                Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
                ProductId = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                DistributionCenterId = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                Quantity = 200
            }
        );
        
        modelBuilder.Entity<Customer>().HasData(
            new Customer
            {
                Id = Guid.Parse("a1111111-1111-1111-1111-111111111111"),
                CustomerName = "Contoso Ltd"
            },
            new Customer
            {
                Id = Guid.Parse("a2222222-2222-2222-2222-222222222222"),
                CustomerName = "Northwind Traders"
            },
            new Customer
            {
                Id = Guid.Parse("a3333333-3333-3333-3333-333333333333"),
                CustomerName = "Adventure Works"
            }
        );
        
        
        modelBuilder.Entity<Product>().HasKey(product => product.SKU);//TODO: because of this LOC /api/inventory/{centerId} doesnt work
        
        modelBuilder.Entity<Product>()//.HasQueryFilter(p => !p.IsDeleted)
            .HasAlternateKey(p => p.Id);

        modelBuilder.Entity<Inventory>().HasQueryFilter(p => !p.IsDeleted)
            .HasOne(i => i.Product)
            .WithMany(p => p.Inventories)
            .HasForeignKey(i => i.ProductId)
            .HasPrincipalKey(p => p.Id);

        modelBuilder.Entity<Inventory>()
            .HasOne(i => i.DistributionCenter)
            .WithMany(d => d.Inventories)
            .HasForeignKey(d => d.DistributionCenterId);

        modelBuilder.Entity<Order>().HasIndex(o => o.CreatedAt);
        modelBuilder.Entity<OrderItem>().HasIndex(oi => oi.OrderId);
        modelBuilder.Entity<Inventory>().HasIndex(i => i.ProductId);
        
        modelBuilder.Entity<Order>().HasQueryFilter(p => !p.IsDeleted && p.Status != OrderStatus.Cancelled)
            .Property(e => e.Status)
            .HasConversion(v => v.ToString(),
            v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v));
        
        modelBuilder.Entity<Shipment>().HasQueryFilter(p => !p.IsDeleted)
            .Property(e => e.Status)
            .HasConversion(v => v.ToString(),
                v => (ShipmentStatus)Enum.Parse(typeof(ShipmentStatus), v));

        modelBuilder.Entity<OrderItem>().HasQueryFilter(p => !p.IsDeleted)
            .HasOne(e => e.Order)
            .WithMany(e => e.Items)
            .HasForeignKey(e => e.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product).WithMany()
            .HasForeignKey(oi => oi.ProductId).HasPrincipalKey(oi => oi.Id);

        modelBuilder.Entity<Customer>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<DistributionCenter>().HasQueryFilter(p => !p.IsDeleted);
    }
}