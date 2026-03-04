using FulfillmentCenter.Data;
using FulfillmentCenter.Entities;
using FulfillmentCenter.Repositories.Interfaces;

namespace FulfillmentCenter.Repositories.Implementations;

public class SqlFulfillmentCenterRepository : IFulfillmentCenterRepository
{
    private FulfillmentCenDbContext _context;
    public SqlFulfillmentCenterRepository()
    {
        _context = new FulfillmentCenDbContext();
    }

    public void Create(Entities.FulfillmentCenter fulfillmentCenter)
    {
        _context.FulfillmentCenters.Add(fulfillmentCenter);
        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var fulfillmentCenterToDelete = _context.FulfillmentCenters.FirstOrDefault(center => center.Id == id);
        _context.FulfillmentCenters.Remove(fulfillmentCenterToDelete);
        _context.SaveChanges();
    }

    public List<Entities.FulfillmentCenter> Read()
    {
        List<Entities.FulfillmentCenter> fulfillmentCenters = _context.FulfillmentCenters.ToList();
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

    public void UpdateFulfillmentCenter<TUpdateParam>(Guid FulfillmentCenterId, TUpdateParam updateParam, Action<TUpdateParam, Entities.FulfillmentCenter> up)
    {
        var fulfillmentCenterToUpdate = _context.FulfillmentCenters.FirstOrDefault(center => center.Id == FulfillmentCenterId);
        up(updateParam, fulfillmentCenterToUpdate);
        _context.SaveChanges();
    }
}