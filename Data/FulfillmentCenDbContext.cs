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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasKey(product => product.SKU);
        
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
    
    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //TODO review: this is a hardcoded connection string - it's not secure. It should be read from appsettings.json (or environment variables) and passed in via the constructor using DbContextOptions<T>
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFCoreExampleDB;Trusted_Connection=True;");
    }*/
}