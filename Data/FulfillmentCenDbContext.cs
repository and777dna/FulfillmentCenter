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
    
    public DbSet<DistributionCenter> DistributionCenter { get; set; }
    public DbSet<Inventory> Inventory { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<Shipment> Shipment { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasKey(product => product.SKU);//TODO: because of this LOC /api/inventory/{centerId} doesnt work
        
        modelBuilder.Entity<Product>()
            .HasAlternateKey(p => p.Id);

        modelBuilder.Entity<Inventory>()
            .HasOne(i => i.Product)
            .WithMany(p => p.Inventory)
            .HasForeignKey(i => i.ProductId)
            .HasPrincipalKey(p => p.Id);
        
        modelBuilder.Entity<Order>()
            .Property(e => e.Status)
            .HasConversion(v => v.ToString(),
            v => (OrderStatus)Enum.Parse(typeof(OrderStatus), v));
        
        modelBuilder.Entity<Shipment>()
            .Property(e => e.Status)
            .HasConversion(v => v.ToString(),
                v => (ShipmentStatus)Enum.Parse(typeof(ShipmentStatus), v));

        modelBuilder.Entity<OrderItem>()
            .HasOne(e => e.Order)
            .WithMany(e => e.Items)
            .HasForeignKey(e => e.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}