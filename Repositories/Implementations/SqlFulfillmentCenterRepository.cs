using FulfillmentCenter.Data;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;

namespace FulfillmentCenter.Repositories.Implementations;

//TODO review: Across all implementations:
//1. DbContext is instantiated with new instead of injected. This is not a good practice. It should be injected via the constructor.
//2. Every Delete method calls FirstOrDefault(...) and immediately passes the result to .Remove() without checking for null. If the entity doesn't exist, this will throw a NullReferenceException or ArgumentNullException
//3. All Read() methods load the entire table into memory as a List<T>. No filtering, no Where, no pagination. This will not scale
//4. No async/await. In an ASP.NET Core app, all I/O should be async (SaveChangesAsync, ToListAsync, etc.) to avoid thread starvation
//5. isCached is set to true in the constructor and never reset to false after Create, Delete, or Update operations. This means after the first load, Read() will always return the stale in-memory list, never re-querying the database
public class SqlFulfillmentCenterRepository : IFulfillmentCenterRepository
{
    private FulfillmentCenDbContext _context;
    public SqlFulfillmentCenterRepository()
    {
        _context = new FulfillmentCenDbContext();
    }

    public void Create(DistributionCenter distributionCenter)
    {
        _context.DistributionCenters.Add(distributionCenter);
        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var fulfillmentCenterToDelete = _context.DistributionCenters.FirstOrDefault(center => center.Id == id);
        _context.DistributionCenters.Remove(fulfillmentCenterToDelete);
        _context.SaveChanges();
    }

    public List<Entities.DistributionCenter> Read()
    {
        List<Entities.DistributionCenter> fulfillmentCenters = _context.DistributionCenters.ToList();
        return fulfillmentCenters;
    }

    public void UpdateInventory(Guid FulfillmentCenterId, Inventory inventory)
    {
        UpdateFulfillmentCenter(FulfillmentCenterId, inventory,
            (inventory, fulfillmentCente) =>
            {
                var InventoryToUpdate = fulfillmentCente.Inventory.FirstOrDefault(inventor => inventor.Id == inventory.Id);
                InventoryToUpdate = inventory; 
            });
    }

    public void UpdateFulfillmentCenter<TUpdateParam>(Guid FulfillmentCenterId, TUpdateParam updateParam, Action<TUpdateParam, Entities.DistributionCenter> up)
    {
        var fulfillmentCenterToUpdate = _context.DistributionCenters.FirstOrDefault(center => center.Id == FulfillmentCenterId);
        up(updateParam, fulfillmentCenterToUpdate);
        _context.SaveChanges();
    }
}