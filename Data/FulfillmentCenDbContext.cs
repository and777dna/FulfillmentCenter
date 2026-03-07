using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Data;

//TODO review: use standart EF pattern for DbContext: public FulfillmentCenDbContext(DbContextOptions<FulfillmentCenDbContext> options) : base(options) { }
//Without this, the context can't be properly registered with AddDbContext<>() in Program.cs, and the connection string can't be injected from configuration
//Since the full word is used everywhere else, this should be FulfillmentCenterDbContext for consistency
public class FulfillmentCenDbContext : DbContext
{
    public DbSet<Entities.DistributionCenter> FulfillmentCenters { get; set; }
    public DbSet<Entities.Inventory> Inventories { get; set; }
    public DbSet<Entities.Order> Orders { get; set; }
    public DbSet<Entities.OrderItem> OrderItems { get; set; }
    public DbSet<Entities.Product> Products { get; set; }
    public DbSet<Entities.Shipment> Shipments { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //TODO review: this is a hardcoded connection string - it's not secure. It should be read from appsettings.json (or environment variables) and passed in via the constructor using DbContextOptions<T>
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFCoreExampleDB;Trusted_Connection=True;");
    }
}