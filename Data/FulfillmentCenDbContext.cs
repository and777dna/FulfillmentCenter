using Microsoft.EntityFrameworkCore;

namespace FulfillmentCenter.Data;

public class FulfillmentCenDbContext : DbContext
{
    public DbSet<Entities.FulfillmentCenter> FulfillmentCenters { get; set; }
    public DbSet<Entities.Inventory> Inventories { get; set; }
    public DbSet<Entities.Order> Orders { get; set; }
    public DbSet<Entities.OrderItem> OrderItems { get; set; }
    public DbSet<Entities.Product> Products { get; set; }
    public DbSet<Entities.Shipment> Shipments { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFCoreExampleDB;Trusted_Connection=True;");
    }
}