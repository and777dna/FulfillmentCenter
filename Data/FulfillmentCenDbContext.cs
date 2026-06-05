using FulfillmentCenter.Entities;
using FulfillmentCenter.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Product A",
                SKU = "SKU-001",
                Weight = 1.5m
            },
            new Product
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Product B",
                SKU = "SKU-002",
                Weight = 2.0m
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

        modelBuilder.Entity<Order>().HasIndex(o => o.CreatedAt);
        modelBuilder.Entity<OrderItem>().HasIndex(oi => oi.OrderId);
        modelBuilder.Entity<Inventory>().HasIndex(i => i.ProductId);
        
        modelBuilder.Entity<Order>().HasQueryFilter(p => !p.IsDeleted)
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

        modelBuilder.Entity<Customer>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<DistributionCenter>().HasQueryFilter(p => !p.IsDeleted);
    }
}